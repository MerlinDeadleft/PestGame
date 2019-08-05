using System.Collections;
using System.Collections.Generic;

namespace ModularAI
{
	namespace BT
	{
		public class Conditional : Node
		{
			ActionNode conditionAction = null;
			Node childNode = null;

			public Conditional(Node node, ActionNode conditionToTest)
			{
				childNode = node;
				conditionAction = conditionToTest;
			}

			public override NodeStates Evaluate()
			{
				NodeState = conditionAction.Evaluate() == NodeStates.Success ? childNode.Evaluate() : NodeStates.Failure;

				return NodeState;
			}
		}
	}
}