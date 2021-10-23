using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BlockingQueue.Tests
{
    [TestClass]
    public class BlockingQueueTests
    {
        [TestMethod]
        public async Task Push_Multithread_CheckCount()
        {
            var blockingQueue = new BlockingQueue<int>();

            var t1 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t2 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t3 = Task.Run(() => PushToQueue(blockingQueue, 1000));

            var result = Task.WhenAll(t1, t2, t3);
            await result;

            Assert.IsTrue(result.IsCompletedSuccessfully);
            Assert.AreEqual(3000, blockingQueue.Count());
        }

        [TestMethod]
        public async Task Pull_Multithread_CheckCount()
        {
            var blockingQueue = new BlockingQueue<int>();
            PushToQueue(blockingQueue, 5000);

            var t1 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t2 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t3 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t4 = Task.Run(() => PullFromQueue(blockingQueue, 1000));

            var result = Task.WhenAll(t1, t2, t3, t4);
            await result;

            Assert.IsTrue(result.IsCompletedSuccessfully);
            Assert.AreEqual(1000, blockingQueue.Count());
        }

        [TestMethod]
        public async Task PullPush_PushFirst_Multithread_CheckCount()
        {
            var blockingQueue = new BlockingQueue<int>();

            var t1 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t2 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t3 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t4 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t5 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t6 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t7 = Task.Run(() => PullFromQueue(blockingQueue, 1000));

            var result = Task.WhenAll(t1, t2, t3, t4, t5, t6, t7);
            await result;

            Assert.IsTrue(result.IsCompletedSuccessfully);
            Assert.AreEqual(1000, blockingQueue.Count());
        }

        [TestMethod]
        public async Task PullPush_PullFirst_Multithread_CheckCount()
        {
            var blockingQueue = new BlockingQueue<int>();

            var t1 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t2 = Task.Run(() => PullFromQueue(blockingQueue, 1000));
            var t3 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t4 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t5 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t6 = Task.Run(() => PushToQueue(blockingQueue, 1000));
            var t7 = Task.Run(() => PullFromQueue(blockingQueue, 1000));

            var result = Task.WhenAll(t1, t2, t3, t4, t5, t6, t7);
            await result;

            Assert.IsTrue(result.IsCompletedSuccessfully);
            Assert.AreEqual(1000, blockingQueue.Count());
        }

        private void PushToQueue(BlockingQueue<int> queue, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                queue.Push(i);
            }
        }

        private void PullFromQueue(BlockingQueue<int> queue, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                queue.Pull();
            }
        }
    }
}
