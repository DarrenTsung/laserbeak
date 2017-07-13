using System.Collections;

using DTObjectPoolManager.Internal;

namespace DTObjectPoolManager {
	public interface IRecycleCleanupSubscriber {
		void OnRecycleCleanup();
	}
}