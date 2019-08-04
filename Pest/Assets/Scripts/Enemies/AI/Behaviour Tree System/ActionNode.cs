using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularAI
{
	namespace BT
	{
		public class ActionNode : Node
		{
			public delegate NodeStates ActionNodeDelegate();

			ActionNodeDelegate action = null;

			public ActionNode(ActionNodeDelegate actionDelegate)
			{
				action = actionDelegate;
			}

			public override NodeStates Evaluate()
			{
				switch(action())
				{
					case NodeStates.Success:
						NodeState = NodeStates.Success;
						return NodeState;
					case NodeStates.Running:
						NodeState = NodeStates.Running;
						return NodeState;
					case NodeStates.Failure:
						NodeState = NodeStates.Failure;
						return NodeState;
					default:
						NodeState = NodeStates.Failure;
						return NodeState;
				}
			}
		}
	}
}