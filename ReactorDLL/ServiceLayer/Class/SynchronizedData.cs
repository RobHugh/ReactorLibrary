using ServiceLayer.Interface;
using System.Threading;

namespace ServiceLayer.Class
{
    public class SynchronizedData : ISynchronizedData<byte[]>
    {
        private ReaderWriterLockSlim dataLock = new ReaderWriterLockSlim();
        private byte[] data;

        public byte[] ReadData()
        {
            dataLock.EnterReadLock();
            try
            {
                return data;
            }
            finally
            {
                dataLock.ExitReadLock();
            }
        }

        public void WriteData(byte[] data)
        {
            dataLock.EnterWriteLock();
            try
            {
                this.data = data;
            }
            finally
            {
                dataLock.ExitWriteLock();
            }            
        }
    }
}
