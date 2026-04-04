using ICities;

namespace KianCommons.IImplict;

internal interface IModWithSettings : IUserMod
{
	void OnSettingsUI(UIHelper helper);
}
