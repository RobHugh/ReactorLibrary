namespace ServiceLayer.Interface
{
    public interface ISynchronizedData<TDataType>
    {
        TDataType ReadData();
        void WriteData(TDataType data);
    }
}
