using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class AppConstants
    {
        public const string OverlayManager = "overlay.manager";
        public const string FormatStrings = "format.strings";
        public const string OverlayContent = "overlay.content";
        public const string StateManager = "state.manager";
        public const string ImageMaps = "image.maps";
        public const string Connections = "devtool.connections";
        public const string ConnectionsFilepath = "connections.xml";
        public const string Workspace = "current.workspace";
        public const string WorkspaceFilename = "xtool.workspace.xml";
        public const string XToolWorkspaceViewModel = "xtool.workspace.vm";
        public const string AsciiMaps = "ascii.maps";
        public const string Candidate = "connections.filepath";
        public const string ExplorerFoldername = "sqlexplorer";
        public const string RedisKnownTypesFoldername = "redis.knowntypes";
        public const string ExplorerFilename = "sql";
        public const string DefaultUrl = "{api/url}";
        //public const string DefaultUrl2 = "{api/url}";

        public static IList<string> DefaultUrls = new List<string>()
        {
            DefaultUrl,
            "rootUrl",
        };

        public static IDictionary<string, ConnectionInfoTypeOption> ProviderNames = new Dictionary<string, ConnectionInfoTypeOption>(StringComparer.OrdinalIgnoreCase)
        {
            {"None",ConnectionInfoTypeOption.None},
            {"HttpClient",ConnectionInfoTypeOption.Api},
            {"System.Data.SqlClient",ConnectionInfoTypeOption.SqlServer},
            {"MongoDb.Driver",ConnectionInfoTypeOption.MongoDb },
            {"StackExchange.Redis",ConnectionInfoTypeOption.Redis},
            {"MySql.Data.MySqlClient",ConnectionInfoTypeOption.MySql},
            {"Neo4jClient",ConnectionInfoTypeOption.Neo4j},
            {"XF.FileSystem.Client",ConnectionInfoTypeOption.File},
            {"ExcelHelper",ConnectionInfoTypeOption.Excel},
            {"System.Xml",ConnectionInfoTypeOption.Xml},
            {"System.Io",ConnectionInfoTypeOption.Json},

        };

        public static IDictionary<ConnectionInfoTypeOption, string> Providers = new Dictionary<ConnectionInfoTypeOption, string>
        {
            {ConnectionInfoTypeOption.None,"None"},
            {ConnectionInfoTypeOption.Api,"HttpClient" },
            {ConnectionInfoTypeOption.SqlServer,"System.Data.SqlClient"},
            {ConnectionInfoTypeOption.MongoDb,"MongoDb.Driver"},
            {ConnectionInfoTypeOption.Redis,"StackExchange.Redis"},
            {ConnectionInfoTypeOption.MySql,"MySql.Data.MySqlClient"},
            {ConnectionInfoTypeOption.Neo4j,"Neo4jClient"},
            {ConnectionInfoTypeOption.File,"XF.FileSystem.Client"},
            {ConnectionInfoTypeOption.Excel,"ExcelHelper"},
            {ConnectionInfoTypeOption.Xml,"System.Xml"},
            {ConnectionInfoTypeOption.Json,"System.Io"},


        };

    }
}
