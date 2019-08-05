using System.Collections;
using System.Collections.Generic;

namespace ModularAI
{
	namespace BT
	{
		public class Selector : Node
		{
			protected List<Node> childNodes = new List<Node>();

			public Selector(List<Node> nodes)
			{
				childNodes = nodes;
			}

			public override NodeStates Evaluate()
			{
				foreach(Node node in childNodes)
				{
					switch(node.Evaluate())
					{
						case NodeStates.Success:
							NodeState = NodeStates.Success;
							return NodeState;
						case NodeStates.Running:
							NodeState = NodeStates.Running;
							return NodeState;
						case NodeStates.Failure:
							continue;
						default:
							continue;
					}
				}
				NodeState = NodeStates.Failure;
				return NodeState;
			}
		}
	}
}