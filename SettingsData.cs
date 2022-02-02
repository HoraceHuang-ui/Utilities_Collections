using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Utilities_Fix
{
    class SettingsData
    {
        // General
        public int display_theme;
        public bool language_eng;
        public bool pane_top;
        public bool first_time_open;

        // freecalc
        public bool degree;
        public int decimal_accuration;

        // tianapi
        public bool custom_apikey_switch;
        public string apikey;

        // bing wallpaper
        public bool custom_mkt_switch;
        public string mkt;
        public int resolution;

        public SettingsData()
        {
            display_theme = 2;
            language_eng = false;
            pane_top = false;
            first_time_open = true;
            degree = false;
            decimal_accuration = 5;
            custom_apikey_switch = false;
            apikey = "a9c1ad156691364ebfe8b3f1ff4eb153";
            custom_mkt_switch = false ;
            mkt = "zh-CN";
            resolution = 1;
        }

        public string GetDataFrom(string yaml, string arg)
        {
            string res = "";
            bool start = false;
            for (int i = 0; i < yaml.Length - arg.Length + 1; i++)
            {
                if (yaml.Substring(i, arg.Length + 1) == arg+":")
                {
                    i += arg.Length + 2;
                    start = true;
                }
                if (start)
                {
                    while (yaml[i] != '\n' && yaml[i] != '\r')
                    {
                        res += yaml[i];
                        i++;
                    }
                    return res;
                }
            }
            return "";
        }

        public async void SyncPreviousSettings(StorageFile file)
        {
            string yaml = await FileIO.ReadTextAsync(file);
            display_theme = GetDataFrom(yaml, "display_theme")[0] - '0';
            language_eng = GetDataFrom(yaml, "language") == "1";
            pane_top = GetDataFrom(yaml, "pane_top") == "1";
            degree = GetDataFrom(yaml, "degree") == "1";
            decimal_accuration = int.Parse(GetDataFrom(yaml, "decimal_accuration"));
            custom_apikey_switch = GetDataFrom(yaml, "custom_apikey_switch") == "1";
            apikey = GetDataFrom(yaml, "apikey");
            custom_mkt_switch = GetDataFrom(yaml, "custom_mkt_switch") == "1";
            mkt = GetDataFrom(yaml, "mkt");
            resolution = GetDataFrom(yaml, "resolution")[0] - '0';
        }

        /// <summary>
        /// 从 f_path.txt 中读取 Yaml 格式的设置数据，若本地无数据则初始化。
        /// </summary>
        public async Task GetLocalSettingsAsync()
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            StorageFolder folder = KnownFolders.DocumentsLibrary;
            StorageFile file = await folder.CreateFileAsync("utilities\\datav1_1_0.txt", CreationCollisionOption.OpenIfExists);

            string yaml_data = await FileIO.ReadTextAsync(file);
            SettingsData sd = deserializer.Deserialize<SettingsData>(yaml_data);
            if (sd == null)
            {
                yaml_data =
                    "display_theme: 2\n" +
                    "language_eng: false\n" +
                    "pane_top: false\n" +
                    "first_time_open: true\n" +
                    "degree: false\n" +
                    "decimal_accuration: 5\n" +
                    "custom_apikey_switch: false\n" +
                    "apikey: a9c1ad156691364ebfe8b3f1ff4eb153\n" +
                    "custom_mkt_switch: false\n" +
                    "mkt: zh-CN\n" +
                    "resolution: 1\n";
                await FileIO.WriteTextAsync(file, yaml_data);
                sd = deserializer.Deserialize<SettingsData>(yaml_data);
                CopyDataFrom(sd);
                try
                {
                    StorageFile prevFile = await folder.GetFileAsync("utilities\\datav1_0_0.txt");
                    SyncPreviousSettings(prevFile);
                    await RefreshLocalFileAsync();
                }
                catch
                {
                    
                }
            }
            else
            {
                CopyDataFrom(sd);
            }
        }

        public void CopyDataFrom(SettingsData a)
        {
            display_theme = a.display_theme;
            language_eng = a.language_eng;
            pane_top = a.pane_top;
            first_time_open = a.first_time_open;
            degree = a.degree;
            decimal_accuration = a.decimal_accuration;
            custom_apikey_switch = a.custom_apikey_switch;
            apikey = a.apikey;
            custom_mkt_switch = a.custom_mkt_switch;
            mkt = a.mkt;
            resolution = a.resolution;
        }
        /// <summary>
        /// 改了某一个值之后保存到本地用
        /// </summary>
        /// <returns></returns>
        public async Task RefreshLocalFileAsync()
        {
            StorageFolder folder = KnownFolders.DocumentsLibrary;
            StorageFile file = await folder.GetFileAsync("utilities\\datav1_1_0.txt");
            var serializer = new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            string s = serializer.Serialize(this);
            await FileIO.WriteTextAsync(file, s);
        }
    }
}
