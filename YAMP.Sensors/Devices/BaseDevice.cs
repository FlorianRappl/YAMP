namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// The base of all devices.
    /// </summary>
    public abstract class BaseDevice
    {
        Int32 _readingChangedHandlers;

        /// <summary>
        /// Installs the device specific handler (or not).
        /// </summary>
        /// <param name="sensor"></param>
        protected void InstallHandler(Object sensor)
        {
            if (sensor != null && _readingChangedHandlers == 0)
            {
                InstallReadingChangedHandler();
            }

            _readingChangedHandlers++;
        }

        /// <summary>
        /// Uninstalls the device specific handler (or not).
        /// </summary>
        protected void UninstallHandler(Object sensor)
        {
            _readingChangedHandlers--;

            if (sensor != null && _readingChangedHandlers == 0)
            {
                UninstallReadingChangedHandler();
            }
        }

        /// <summary>
        /// Installs the reading handler.
        /// </summary>
        protected abstract void InstallReadingChangedHandler();

        /// <summary>
        /// Uninstalls the reading handler.
        /// </summary>
        protected abstract void UninstallReadingChangedHandler();
    }
}
