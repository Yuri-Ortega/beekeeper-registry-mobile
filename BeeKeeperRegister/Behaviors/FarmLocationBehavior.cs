using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace BeeKeeperRegister.Behaviors
{
    public class FarmLocationBehavior : Behavior<Map>
    {
        public static readonly BindableProperty LatitudeProperty =
        BindableProperty.Create(nameof(Latitude), typeof(double), typeof(FarmLocationBehavior), 0.0, propertyChanged: OnLocationChanged);

        public static readonly BindableProperty LongitudeProperty =
            BindableProperty.Create(nameof(Longitude), typeof(double), typeof(FarmLocationBehavior), 0.0, propertyChanged: OnLocationChanged);

        public double Latitude
        {
            get => (double)GetValue(LatitudeProperty);
            set => SetValue(LatitudeProperty, value);
        }

        public double Longitude
        {
            get => (double)GetValue(LongitudeProperty);
            set => SetValue(LongitudeProperty, value);
        }

        private static void OnLocationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not FarmLocationBehavior behavior) return;
            if (behavior.AssociatedObject is not Map map) return;
            if (behavior.Latitude == 0 || behavior.Longitude == 0) return;

            var location = new Location(behavior.Latitude, behavior.Longitude);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(600)));
            map.Pins.Clear();
            map.Pins.Add(new Pin { Label = "Farm Location", Location = location });
        }

        protected Map? AssociatedObject { get; private set; }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        private void OnBindingContextChanged(object? sender, EventArgs e)
        {
            BindingContext = AssociatedObject?.BindingContext;
        }
    }
}
