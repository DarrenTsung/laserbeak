using System.Collections;

using DTObjectPoolManager.Internal;

namespace DTObjectPoolManager {
	public interface IRecycleSetupSubscriber {
		void OnRecycleSetup();
	}
}