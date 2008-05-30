// CheckForSapiEngineNative.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "CheckForSapiEngineNative.h"

int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	HKEY key;
	if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, L"Software\\Microsoft\\Speech\\Recognizers\\Tokens", 0, KEY_ENUMERATE_SUB_KEYS, &key) != ERROR_SUCCESS)
	{
		return 1;
	}

	wchar_t keyName[101];
	DWORD length = 101;
	LONG result = RegEnumKeyEx(key, 0, keyName, &length, NULL, NULL, NULL, NULL);

	RegCloseKey(key);

	if (result == ERROR_SUCCESS)
	{
		return 0;
	}
	else
	{
		return 1;
	}
}
