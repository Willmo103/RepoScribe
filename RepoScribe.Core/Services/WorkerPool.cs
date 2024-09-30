using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RepoScribe.Core.Services
{
    public class WorkerPool
    {
        private readonly BlockingCollection<Func<Task>> _taskQueue = new BlockingCollection<Func<Task>>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public WorkerPool(int workerCount)
        {
            for (int i = 0; i < workerCount; i++)
            {
                Task.Factory.StartNew(async () =>
                {
                    foreach (var workItem in _taskQueue.GetConsumingEnumerable(_cts.Token))
                    {
                        try
                        {
                            await workItem();
                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions (log or rethrow)
                        }
                    }
                }, TaskCreationOptions.LongRunning);
            }
        }

        public void EnqueueTask(Func<Task> task)
        {
            _taskQueue.Add(task);
        }

        public void Stop()
        {
            _cts.Cancel();
            _taskQueue.CompleteAdding();
        }
    }
}
