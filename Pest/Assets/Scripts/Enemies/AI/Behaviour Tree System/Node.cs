using System.Collections;
using System.Collections.Generic;

namespace ModularAI
{
	namespace BT
	{
		public enum NodeStates { Success, Running, Failure };

		[System.Serializable]
		public abstract class Node
		{
			public NodeStates NodeState { get; protected set; }

			public abstract NodeStates Evaluate();
		}
	}
}