using ElysiumAttributes;

namespace ElysiumInterfaces
{
	[ExcludeFromNodeBuild]
	public interface IFieldObject
	{
		public void OnValueModified(string newValue);
		public object TransmitToNode();
	}
}