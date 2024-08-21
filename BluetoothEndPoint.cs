namespace OBDIIApp
{
    internal class BluetoothEndPoint
    {
        private object value;
        private object serialPort;

        public BluetoothEndPoint(object value, object serialPort)
        {
            this.value = value;
            this.serialPort = serialPort;
        }
    }
}