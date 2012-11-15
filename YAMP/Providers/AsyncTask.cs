using System;
using System.ComponentModel;

namespace YAMP
{
    class AsyncTask : BackgroundWorker
    {
        public Action<QueryContext, Exception> Continuation { get; set; }
    }
}