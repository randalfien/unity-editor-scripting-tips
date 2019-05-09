From:
https://github.com/wlgys8/UnityShellHelper

example:

	ShellHelper.ShellRequest req = ShellHelper.ProcessCommand("ls","");
	req.onLog += delegate(int logType, string log) {
		Debug.Log(arg2);
	}; 