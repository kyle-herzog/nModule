using System;
using System.Collections.Generic;

namespace nModule
{
	/// <summary>
	/// The base type for dynamically interchangable modular code at runtime.
	/// </summary>
    public abstract class Module : IModule
	{
		#region Static Members

		private static Random ModuleIdGenerator = new Random();
		private static List<int> ModuleIds = new List<int>();

		private static int GenerateModuleId()
		{
			int nextId;
			lock(ModuleIdGenerator)
			{
				do
				{
					nextId = ModuleIdGenerator.Next();
				} 
				while (ModuleIds.Contains(nextId));
				ModuleIds.Add(nextId);
			}
			return nextId;
		}
		
		#endregion Static Members

		private string _moduleName;
		private int _moduleId;

		#region Properties

		/// <summary>
		/// The name for this module.
		/// </summary>
        public string ModuleName { get { return _moduleName; } }
		
		/// <summary>
		/// The unique Id for this module.
		/// </summary>
		public int ModuleId { get { return _moduleId; } }

		/// <summary>
		/// When overridden will provide a value describing the module
		/// </summary>
		public abstract string ModuleType { get; }

		/// <summary>
		/// Returns the ModuleInstantiation for this module stating how it may be instantiated via the ModuleManager. May be overridden.
		/// </summary>
		public virtual ModuleInstantiation ModuleInstantiation { get { return ModuleInstantiation.Singleton; } }

		/// <summary>
		/// Defines the priority of this module for comparison against other modules when calculating the "BestModule"
		/// </summary>
		public int ModulePriority { get; set; }

		/// <summary>
		/// A internal getter and setter for the public description of the module's current status.
		/// </summary>
		protected string InternalModuleStatus { get; set; }

		/// <summary>
		/// A description of the module's current status.
		/// </summary>
		public string ModuleStatus { get { return InternalModuleStatus; } }

		/// <summary>
		/// The ModuleState accessible by sub types of Module which allows getting and setting of the Module's current state.
		/// </summary>
		protected ModuleState InternalModuleState { get; set; }

		/// <summary>
		/// The current state of the Module.
		/// </summary>
		public ModuleState ModuleState { get { return InternalModuleState; } }

		/// <summary>
		/// Value stating whether or not this Module has been Disposed
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// Value stating whether or not this module is being Disposed
		/// </summary>
		public bool IsDisposing { get; private set; }

		#endregion Properties

		#region Constructors

		/// <summary>
		/// Initializes the Module with the base name ""
		/// </summary>
		protected Module()
			: this("")
		{

		}

		/// <summary>
		/// Initializes the Module with the specified name
		/// </summary>
		/// <param name="name">The name that this module should have.</param>
		protected Module(string name)
        {
			ModulePriority = 0;
            _moduleName = name;
			_moduleId = GenerateModuleId();
			InternalModuleStatus = "Instantiated";
			InternalModuleState = ModuleState.NotInitialized;
		}

		#endregion Constructors


		#region Methods

		/// <summary>
		/// Initializes the Module to a working state.
		/// </summary>
		public void Initialize()
		{
			InternalModuleState = ModuleState.Initializing;
			InternalModuleStatus = "The Module is now initializing.";
			try
			{
				OnInitialize();
				InternalModuleState = ModuleState.Healthy;
				InternalModuleStatus = "The Module is now initialized";
			}
			catch (Exception ex)
			{
				InternalModuleState = ModuleState.Error;
				InternalModuleStatus = String.Format("An error occurred when initializing the module {0}: {1}", ModuleName, ex.Message);
			}
		}

		/// <summary>
		/// When overridden in a derived class, initializes the module to a working state. 
		/// </summary>
        protected internal virtual void OnInitialize() 
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			IsDisposing = true;
            try
            {
                OnDispose();
			    IsDisposed = true;
            }
            catch
            {
                InternalModuleStatus = "An error occured when disposing the module";
            }
			IsDisposing = false;
		}

        /// <summary>
        /// When overridden in a subclass this will provide functionality to dispose resources created by the sub-instantiated module.
        /// </summary>
        protected internal virtual void OnDispose() { }

		/// <summary>
		/// 
		/// </summary>
		public void Poll()
		{
			try
			{
				OnPoll();
			}
			catch
			{
				InternalModuleStatus = "An error occured when polling the module.";
				InternalModuleState = ModuleState.Error;
			}
		}

		/// <summary>
		/// When overridden in a subclass this will provide custom polling actions for the sub-instantiated module.
		/// </summary>
		protected internal virtual void OnPoll() { }

		#endregion Methods
	}
}
