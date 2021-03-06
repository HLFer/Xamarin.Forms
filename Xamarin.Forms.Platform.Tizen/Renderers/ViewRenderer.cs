using System;
using System.ComponentModel;
using System.Diagnostics;
using ElmSharp;

namespace Xamarin.Forms.Platform.Tizen
{
	/// <summary>
	/// Base class for view renderers.
	/// </summary>
	public abstract class ViewRenderer<TView, TNativeView> : VisualElementRenderer<TView>
		where TView : View
		where TNativeView : EvasObject
	{
		readonly Lazy<GestureDetector> _gestureDetector;

		internal GestureDetector GestureDetector => _gestureDetector.Value;

		/// <summary>
		/// Default constructor.
		/// </summary>
		protected ViewRenderer()
		{
			_gestureDetector = new Lazy<GestureDetector>(() => new GestureDetector(this));
		}

		protected override void OnElementChanged(ElementChangedEventArgs<TView> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null && _gestureDetector.IsValueCreated)
			{
				_gestureDetector.Value.Clear();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
			{
				UpdateIsEnabled();
			}
		}

		/// <summary>
		/// Native control associated with this renderer.
		/// </summary>
		public TNativeView Control
		{
			get
			{
				return (TNativeView)NativeView;
			}
		}

		protected void SetNativeControl(TNativeView control)
		{
			Debug.Assert(control != null);
			SetNativeView(control);
		}

		void UpdateIsEnabled()
		{
			_gestureDetector.Value.IsEnabled = Element.IsEnabled;
		}
	}
}