namespace Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ImageConvertersProvider
    {
        public List<IImageConverter> ImageConverters { get; set; } = new List<IImageConverter>();

        public ImageConvertersProvider()
        {
            SetupImageConverters();
        }

        private void SetupImageConverters()
        {
            var type = typeof(IImageConverter);
            
            ImageConverters = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => (IImageConverter) Activator.CreateInstance(t))
                .ToList();
        }
    }
}