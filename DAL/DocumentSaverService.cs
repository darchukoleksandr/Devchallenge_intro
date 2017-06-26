using System.Collections.ObjectModel;
using System.Threading;

namespace DAL
{
    /// <summary>
    /// Singleton service to parse PDF files. 
    /// Singleton in this case is required for step by step parsing to avoid data multiplying.
    /// </summary>
    public sealed class DocumentSaverService
    {
        #region Singleton
        private static readonly object _lock = new object();
        private static DocumentSaverService _instance;

        private DocumentSaverService()
        {

        }

        public static DocumentSaverService Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lock)
                {
                    DocumentSaverService temp = new DocumentSaverService();
                    Interlocked.Exchange(ref _instance, temp);
                    return _instance;
                }
            }
        }
//            public static DocumentSaverService Instance
//        {
//            get
//            {
//                if (_instance != null)
//                    return _instance;
//                Monitor.Enter(_lock);
//                DocumentSaverService temp = new DocumentSaverService();
//                Interlocked.Exchange(ref _instance, temp);
//                Monitor.Exit(_lock);
//                return _instance;
//            }
//        }
        #endregion

        #region ServiceLogic

        private readonly object _lock2 = new object();
        private bool _readyFlag = true;

        private void DocumentsCollectionOnCollectionChanged()
        {
            while (DocumentsCollection.Count > 0)
            {
                if (!_readyFlag)
                continue;

                _readyFlag = false;
                new PdfExtractor(DocumentsCollection[0]).ExtractAndCleanTextFromPdf();
                DocumentsCollection.RemoveAt(0);
                _readyFlag = true;
            }
        }

        private static readonly ObservableCollection<string> DocumentsCollection = new ObservableCollection<string>();
        /// <summary>
        /// Add the filepath to list and initializes <code>PdfExtractor</code> instance
        /// </summary>
        /// <param name="documentPath"></param>
        public void AddDocument(string documentPath)
        {
            DocumentsCollection.Add(documentPath);
            lock (_lock2)
            {
                DocumentsCollectionOnCollectionChanged();
            }
        }

        #endregion
    }
}
