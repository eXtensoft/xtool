using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTool
{
    public class OverlayManager
    {
        Dictionary<string, Action<dynamic>> _Overlays = new Dictionary<string, Action<dynamic>>();

        public void SetOverlay(string key, dynamic args)
        {
            if (_Overlays.ContainsKey(key))
            {
                _Overlays[key].Invoke(args);
            }
        }

        public void RemoveOverlay(string key)
        {
            if (_Overlays.ContainsKey(key))
            {
                _Overlays.Remove(key);
            }
        }

        public void RegisterOverlay(string key, Action<dynamic> action)
        {
            if (!_Overlays.ContainsKey(key))
            {
                _Overlays.Add(key, action);
            }
        }
    }
}
