using System;
using System.Linq;
using System.Text.RegularExpressions;
using DAL.Models;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace DAL
{
    internal class PdfExtractor : IDisposable
    {
        private readonly SessionDbContext _sessionDbContext = new SessionDbContext();
        private PDDocument _document;
        private string _text;

        public PdfExtractor(string pathToPdf)
        {
            CheckSessionPdfFile(pathToPdf);
        }
        private void CheckSessionPdfFile(string pathToPdf)
        {
            try
            {
                _document = PDDocument.load(pathToPdf);
                _text = new PDFTextStripper().getText(_document).Replace("\r\n", " ");
                // Видалення зайвого  
//                  _text = Regex.Replace(_text, "\\s+ПІДСУМКИ ГОЛОСУВАННЯ.+?\\(прийнято.\\/.не.прийнято\\) ", "", RegexOptions.Compiled);
//                  _text = Regex.Replace(_text, "Система поіменного голосування.+? Результат голосування .+? Результат голосування ", "");
                if (Regex.IsMatch(_text, "Система.поіменного.голосування.{3}Рада.Голос.{3}Броварська.міська.рада"))
                {
                    throw new ArgumentException("PDF file is not represents a deputy session!");
                }
            }
            catch (java.io.IOException)
            {
                throw new ArgumentException("Uploaded file can not be readed as PDF file!");
            }
            finally
            {
                _document.close();
            }
        }

        public void ExtractAndCleanTextFromPdf()
        {
            //Запобігання повторного вводу данних сессії
//            if (_sessionDbContext.Sessions.Any(session => session.Date == sessionDate))
//                throw new ArgumentException("Information about this session is already stored");

            var session = new Session
            {
                Date = DateTime.Parse(Regex.Match(_text, "\\d{2}\\.\\d{2}\\.\\d{2}", RegexOptions.Compiled).Value),
                Name = Regex.Match(_text, "Броварська.міська.рада.(.*?).від", RegexOptions.Compiled).Groups[1].Value
            };
            _sessionDbContext.Sessions.Add(session);
            var savedDeputies = _sessionDbContext.Deputies.ToList();

            // Розподіл сессій. Кожна комірка являє собою окреме засідання.
            var votings = Regex.Split(_text, "Система.поіменного.голосування.{1,3}Рада.Голос.{1,3}Броварська.міська.рада", RegexOptions.Compiled);
            var vot =  0;
            foreach (var voting in votings)
            {
                Console.WriteLine($"V: {++vot}");
                var deputies = Regex.Matches(voting, " \\d{1,2}.([А-ЯЄІЇа-яєії]+.{1,2}[А-ЯЄІЇа-яєії]+.{1,2}[А-ЯЄІЇа-яєії]+) (Не.голосував|Відсутній|Утримався|За|Проти)", RegexOptions.Compiled);
                var currentVoting = new Voting
                {
                    Session = session,
                    About = Regex.Match(voting, "Результат поіменного голосування: (.*(?=№:))",
                        RegexOptions.Compiled).Groups[1].Value
                };

                for (var j = 0; j < deputies.Count; j++)
                {
                    Match deputy = deputies[j];
                    var depName = Regex.Replace(deputy.Groups[1].Value, @"\s+", " ");
                    var currentDeputy = savedDeputies.FirstOrDefault(dep => dep.Name == depName);
                    if (currentDeputy == null)
                    {
                        currentDeputy = new Deputy
                        {
                            Name = depName,
                            Party = (short)(j + 1)
                        };

                        _sessionDbContext.Deputies.Add(currentDeputy);
                        savedDeputies.Add(currentDeputy);
                    }

                    var deputyVote = new DeputyVote
                    {
                        Deputy = currentDeputy,
                        VoteType = deputy.Groups[2].Value,
                        Voting = currentVoting
                    };

                    _sessionDbContext.DeputyVote.Add(deputyVote);
                    _sessionDbContext.Votings.Add(currentVoting);
                }
            }
            _sessionDbContext.BulkSaveChanges();
            Console.WriteLine("Document saved!");
        }

        public void Dispose()
        {
            _sessionDbContext.Dispose();
        }
    }
}
