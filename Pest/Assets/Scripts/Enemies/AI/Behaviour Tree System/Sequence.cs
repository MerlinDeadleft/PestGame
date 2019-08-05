using System.Collections;
using System.Collections.Generic;

namespace ModularAI
{
	namespace BT
	{
		public class Sequence : Node
		{
			List<Node> childNodes = new List<Node>();

			public Sequence(List<Node> nodes)
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
							continue;
						case NodeStates.Running:
							NodeState = NodeStates.Running;
							return NodeState;
						case NodeStates.Failure:
							NodeState = NodeStates.Failure;
							return NodeState;
						default:
							NodeState = NodeStates.Success;
							return NodeState;
					}
				}

				return NodeState;
			}
		}
	}
}