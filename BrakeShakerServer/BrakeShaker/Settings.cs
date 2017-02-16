using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BrakeShaker.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public float RollingRumble
		{
			get
			{
				return (float)this["RollingRumble"];
			}
			set
			{
				this["RollingRumble"] = value;
			}
		}

		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public float SlidingRumble
		{
			get
			{
				return (float)this["SlidingRumble"];
			}
			set
			{
				this["SlidingRumble"] = value;
			}
		}

		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int tyreChosen
		{
			get
			{
				return (int)this["tyreChosen"];
			}
			set
			{
				this["tyreChosen"] = value;
			}
		}

		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int master
		{
			get
			{
				return (int)this["master"];
			}
			set
			{
				this["master"] = value;
			}
		}

		[DefaultSettingValue("0"), UserScopedSetting, DebuggerNonUserCode]
		public int gamma
		{
			get
			{
				return (int)this["gamma"];
			}
			set
			{
				this["gamma"] = value;
			}
		}
	}
}
