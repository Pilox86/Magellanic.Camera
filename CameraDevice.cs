using Magellanic.Camera.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;

namespace Magellanic.Camera
{
    public class CameraDevice : ICameraDevice
    {
        public MediaCapture ViewFinder { get; set; } = new MediaCapture();

        public void Dispose()
        {
            ViewFinder?.Dispose();
            ViewFinder = null;
        }

        public async Task<DeviceInformation> GetCameraAtPanelLocation(Panel cameraPosition)
        {
            var cameraDevices = await GetCameraDevices();

            return cameraDevices.FirstOrDefault(c => c.EnclosureLocation?.Panel == cameraPosition);
        }

        public async Task<DeviceInformation> GetDefaultCamera()
        {
            var cameraDevices = await GetCameraDevices();

            return cameraDevices.FirstOrDefault();
        }

        public async Task InitialiseCameraAsync(DeviceInformation cameraToInitialise)
        {
            await ViewFinder?.InitializeAsync(
                new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = cameraToInitialise.Id
                });
        }

        private async Task<DeviceInformationCollection> GetCameraDevices()
        {
            return await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
        }
    }
}