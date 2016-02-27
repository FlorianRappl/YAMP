namespace YAMP.Sensors.Devices
{
    using System;

    public abstract class BaseDevice
    {
        Int32 _readingChangedHandlers;

        protected void InstallHandler(Object sensor)
        {
            if (sensor != null && _readingChangedHandlers == 0)
            {
                InstallReadingChangedHandler();
            }

            _readingChangedHandlers++;
        }

        protected void UninstallHandler(Object sensor)
        {
            _readingChangedHandlers--;

            if (sensor != null && _readingChangedHandlers == 0)
            {
                UninstallReadingChangedHandler();
            }
        }

        protected abstract void InstallReadingChangedHandler();

        protected abstract void UninstallReadingChangedHandler();
    }
}
