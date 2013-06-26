using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RavenUVE.Views.Utils
{
    public static class ListView
    {

        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached("AutoScroll", 
            typeof(bool), typeof(System.Windows.Controls.ListView), new PropertyMetadata(false));

        public static readonly DependencyProperty AutoScrollHandlerProperty = DependencyProperty.RegisterAttached("AutoScrollHandler", 
            typeof(AutoScrollHandler), typeof(System.Windows.Controls.ListView));

        public static bool GetAutoScroll(System.Windows.Controls.ListView instance)
        {
            Contract.Requires(null != instance);
            return (bool)instance.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(System.Windows.Controls.ListView instance, bool value)
        {
            Contract.Requires(null != instance);
            var oldHandler = instance.GetValue(AutoScrollHandlerProperty) as AutoScrollHandler;
            if (oldHandler != null)
            {
                oldHandler.Dispose();
                instance.SetValue(AutoScrollHandlerProperty, null);
            }

            instance.SetValue(AutoScrollProperty, value);

            if (value)
            {
                instance.SetValue(AutoScrollHandlerProperty, new AutoScrollHandler(instance));
            }
        }

    }

    public class AutoScrollHandler : DependencyObject, IDisposable
    {

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", 
            typeof(IEnumerable), typeof(AutoScrollHandler),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(ItemsSourcePropertyChanged)));

        private System.Windows.Controls.ListView target;

        public AutoScrollHandler(System.Windows.Controls.ListView target)
        {
            this.target = target;
            var binding = new Binding("ItemsSource");
            binding.Source = target;
            BindingOperations.SetBinding(this, ItemsSourceProperty, binding);
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ItemsSourceProperty);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        
        private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var autoScrollHandler = d as AutoScrollHandler;
            if (autoScrollHandler != null)
            {
                autoScrollHandler.ItemsSourceChanged(e.OldValue as INotifyCollectionChanged, e.NewValue as INotifyCollectionChanged);
            }
        }

        void ItemsSourceChanged(INotifyCollectionChanged oldValue, INotifyCollectionChanged newValue)
        {
            if (null != oldValue)
            {
                oldValue.CollectionChanged -= listViewCollectionChanged;
            }
            if (null != newValue)
            {
                newValue.CollectionChanged += listViewCollectionChanged;
                target.ScrollIntoView(target.Items[target.Items.Count -1]);
            }
        }

        private void listViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add || e.NewItems == null || e.NewItems.Count < 1)
            {
                return;
            }

            target.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
        }


        
    }

}
