using QRCoder;
using System.Drawing;

namespace IC.CrossCutting
{
    public static class QRCodeService
    {
        public static byte[] GetQRCode(string json)
        {
            var bitmap = GetBitmapQRCode(json);
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
        }

        public static Bitmap GetBitmapQRCode(string json)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}