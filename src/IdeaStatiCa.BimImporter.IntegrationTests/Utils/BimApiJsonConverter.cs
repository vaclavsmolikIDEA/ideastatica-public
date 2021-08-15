using IdeaStatiCa.BimApi;
using IdeaStatiCa.BimApi.Results;
using IdeaStatiCa.BimImporter.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IdeaStatiCa.BimImporter.IntegrationTests.Utils
{
	internal class BimApiJsonConverter : JsonConverter
	{
		private readonly Map<int, object> _instances;
		private int _nextId = 0;

		public BimApiJsonConverter(Map<int, object> instances)
		{
			_instances = instances;

			foreach (int id in instances.GetRights())
			{
				_nextId = Math.Max(_nextId, id + 1);
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return !objectType.IsEnum && !objectType.IsArray && !objectType.FullName.StartsWith("System");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object obj = Substitute.For(new Type[] { objectType }, new object[0]);
			JObject jObj = JObject.Load(reader);

			JToken jRefId = jObj.GetValue("$ref");
			if (jRefId != null)
			{
				return _instances.GetRight((int)jRefId);
			}

			JToken id = jObj.GetValue("$id");
			_instances.Add((int)id, obj);

			if (obj is IIdeaObjectWithResults objectWithResults)
			{
				JToken jResults = jObj.GetValue("$results");
				if (jResults != null)
				{
					var results = jResults.ToObject<IEnumerable<IIdeaResult>>(serializer);
					objectWithResults.GetResults().Returns(results);
				}
			}

			foreach (KeyValuePair<string, JToken> kv in jObj)
			{
				if (kv.Key.StartsWith("$"))
				{
					continue;
				}

				PropertyInfo propertyInfo = objectType.GetProperty(kv.Key);
				object value = kv.Value.ToObject(propertyInfo.PropertyType, serializer);

				ReturnValue returnValue = new ReturnValue(value);

				MethodInfo getter = propertyInfo.GetGetMethod();
				getter.Invoke(obj, new object[0]);
				SubstitutionContext.Current.ThreadContext.LastCallShouldReturn(returnValue, MatchArgs.AsSpecifiedInCall);
			}

			return obj;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			if (_instances.TryGetLeft(value, out int refId))
			{
				writer.WritePropertyName("$ref");
				writer.WriteValue(refId);
			}
			else
			{
				Type type = value.GetType();
				List<Type> bimApiInterfaces = type
					.GetInterfaces()
					.Where(x => x.FullName.StartsWith("IdeaStatiCa.BimApi"))
					.ToList();

				if (bimApiInterfaces.Count == 0)
				{
					WriteProperties(writer, value, serializer, type);
				}
				else
				{
					writer.WritePropertyName("$id");
					writer.WriteValue(_nextId);
					_instances.Add(_nextId, value);
					_nextId++;

					writer.WritePropertyName("$type");
					writer.WriteValue(type.FullName);

					foreach (var iface in bimApiInterfaces)
					{
						WriteProperties(writer, value, serializer, iface);
					}

					if (value is IIdeaObjectWithResults objectWithResults)
					{
						writer.WritePropertyName("$results");
						serializer.Serialize(writer, objectWithResults.GetResults());
					}
				}
			}

			writer.WriteEndObject();
		}

		private static void WriteProperties(JsonWriter writer, object value, JsonSerializer serializer, Type type)
		{
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				writer.WritePropertyName(propertyInfo.Name);
				serializer.Serialize(writer, propertyInfo.GetValue(value));
			}
		}
	}
}