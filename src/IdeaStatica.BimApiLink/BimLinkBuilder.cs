//using IdeaStatiCa.Plugin;

//namespace IdeaStatica.BimApiLink
//{
//	public class BimLinkBuilder
//	{
//		private string? _ideaStatiCaPath;
//		private IPluginLogger? _pluginLogger;

//		private readonly IApplicationBIM _applicationBIM;
//		private readonly string _applicationName;
//		private readonly string _checkbotProjectPath;

//		public BimLinkBuilder(IApplicationBIM applicationBIM, string applicationName, string checkbotProjectPath)
//		{
//			_applicationBIM = applicationBIM;
//			_applicationName = applicationName;
//			_checkbotProjectPath = checkbotProjectPath;
//		}

//		public BimLinkBuilder WithIdeaStatiCa(string path)
//		{
//			_ideaStatiCaPath = path;
//			return this;
//		}

//		public BimLinkBuilder WithLogger(IPluginLogger pluginLogger)
//		{
//			_pluginLogger = pluginLogger;
//			return this;
//		}

//		public IBIMPluginHosting Create()
//		{
//			IPluginLogger pluginLogger = _pluginLogger ?? new NullLogger();
//			PluginFactory pluginFactory = new(
//				_applicationBIM,
//				_applicationName,
//				_ideaStatiCaPath);

//			return new GrpcBimHostingFactory(pluginFactory, pluginLogger);
//		}

//		private sealed class PluginFactory : IBIMPluginFactory
//		{
//			public string FeaAppName { get; }

//			public string IdeaStaticaAppPath { get; }

//			private readonly IApplicationBIM _application;

//			public PluginFactory(IApplicationBIM application, string applicationName, string ideaStatiCaPath)
//			{
//				_application = application;
//				FeaAppName = applicationName;
//				IdeaStaticaAppPath = ideaStatiCaPath;
//			}

//			public IApplicationBIM Create()
//				=> _application;
//		}
//	}
//}