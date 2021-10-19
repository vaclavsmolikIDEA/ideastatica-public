#if NETSTANDARD
using System;
using System.Collections.Generic;
using System.Text;

namespace System.ServiceModel
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class OperationContractAttribute : Attribute
	{
		public bool IsOneWay { get; set; }
	}

	public class ServiceContractAttribute : Attribute
	{
	}

}
#endif