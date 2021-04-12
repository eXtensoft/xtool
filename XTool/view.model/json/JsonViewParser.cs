using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class JsonViewParser
    {
        public static bool Parse(string input, out JTokenViewModel viewModel, out string message)
        {
            bool b = false;
            message = "okay";
            viewModel = null;

            if (!String.IsNullOrWhiteSpace(input))
            {
                string s = input.Trim();
                string json = (s.StartsWith("[") && s.EndsWith("]")) ? "{'data': " + s + "}" : s;

                try
                {
                    JObject jObject = JObject.Parse(json);
                    Parse(jObject, null, out viewModel);
                    b = true;
                }
                catch (Exception ex)
                {
                    message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
            return b;
        }

        private static void Parse(JToken token, JToken master, out JTokenViewModel viewModel)
        {

            viewModel = null;
            var type = token.GetType();
            if (type.Equals(typeof(JObject)))
            {
                var prop = token as JObject;
                viewModel = new JObjectViewModel() { Name = token.Path };

                foreach (JToken minion in prop.Children())
                {
                    JTokenViewModel minionVM;
                    Parse(minion, prop, out minionVM);
                    if (minionVM != null && viewModel != null)
                    {
                        if (viewModel.Items == null)
                        {
                            viewModel.Items = new ObservableCollection<JTokenViewModel>();
                        }
                        viewModel.Items.Add(minionVM);
                    }
                }
            }
            else if (type.Equals(typeof(JProperty)))
            {

                var prop = token as JProperty;

                var propType = prop.Value.GetType();
                if (propType.Equals(typeof(JValue)))
                {
                    var propValue = prop.Value as JValue;
                    viewModel = new JPropertyViewModel() { Name = prop.Name, Value = propValue };

                }
                else if (propType.Equals(typeof(JArray)))
                {
                    viewModel = new JArrayViewModel() { Name = prop.Name };
                    var arrayProp = prop.Value as JArray;
                    foreach (var minion in arrayProp.Children())
                    {
                        var minionType = minion.GetType();
                        JTokenViewModel minionVM;
                        Parse(minion, arrayProp, out minionVM);
                        if (minionVM != null)
                        {
                            if (viewModel.Items == null)
                            {
                                viewModel.Items = new ObservableCollection<JTokenViewModel>();
                            }
                            viewModel.Items.Add(minionVM);
                        }
                    }
                }
                else if (propType.Equals(typeof(JObject)))
                {

                    var objProp = prop.Value as JObject;
                    viewModel = new JObjectViewModel() { Name = objProp.Path };
                    foreach (JToken minion in objProp.Children())
                    {
                        JTokenViewModel minionVM;
                        Parse(minion, objProp, out minionVM);
                        if (minionVM != null && viewModel != null)
                        {
                            if (viewModel.Items == null)
                            {
                                viewModel.Items = new ObservableCollection<JTokenViewModel>();
                            }
                            viewModel.Items.Add(minionVM);
                        }
                    }
                }
            }
            else if (type.Equals(typeof(JArray)))
            {
                var prop = token as JArray;
                foreach (var minion in prop.Children())
                {
                    JTokenViewModel minionVM;
                    Parse(minion, prop, out minionVM);
                    if (minionVM != null && viewModel != null)
                    {
                        if (viewModel.Items == null)
                        {
                            viewModel.Items = new ObservableCollection<JTokenViewModel>();
                        }
                        viewModel.Items.Add(minionVM);
                    }
                }
            }
            else
            {
                var prop = token as JValue;
                viewModel = new JPropertyViewModel() { Name = prop.Path, Value = prop.Value };
            }
        }
    }
}
