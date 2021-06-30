import json
import os
import shlex
import subprocess
import sys


def call(cmd: str, printOutput: bool = True) -> tuple[str, bool]:
    # print(f"{printOutput = }, {cmd = }")
    if sys.platform == "win32":
        args = cmd
    else:
        # linux must split arguments
        args = shlex.split(cmd)
    try:
        if printOutput:
            isOk = subprocess.call(args) == 0
            return "", isOk

        data = subprocess.check_output(args)
        # python3 output is bytes
        output = data.decode("utf-8")
        return output, True
    except subprocess.CalledProcessError as callerr:
        print(f"cmd = {cmd}, callerr.output = {callerr.output}", file=sys.stderr)
        return (callerr.output, False)
    except IOError as ioerr:
        print(f"cmd = {cmd}, ioerr = {ioerr}", file=sys.stderr)
        return "", False


def get_project_dir():
    BASE_DIR = os.path.dirname(__file__)
    BASE_DIR = os.path.dirname(BASE_DIR)
    while BASE_DIR:
        BASE_DIR = os.path.dirname(BASE_DIR)
        if not BASE_DIR:
            return None
        assets_dir = os.path.join(BASE_DIR, "Assets")
        projectsettings_dir = os.path.join(BASE_DIR, "ProjectSettings")
        if os.path.exists(assets_dir) and os.path.exists(projectsettings_dir):
            return BASE_DIR
    return None

def get_script_file(project_dir):
    setting_file = os.path.join(project_dir, "UserSettings/LuaInteractive.json")
    if os.path.exists(setting_file):
        with open(setting_file, mode='r', encoding='utf-8') as f:
            setting = json.loads(f.read())
            script_file = setting.get('scriptPath', '')
            if script_file:
                script_file = os.path.join(project_dir, script_file)
                if os.path.exists(script_file):
                    return script_file

    return None

def get_package_name(project_dir):
    import yaml
    setting_file = os.path.join(project_dir, "ProjectSettings/ProjectSettings.asset")
    with open(setting_file, mode='r', encoding='utf-8') as f:
        f.readline()
        f.readline()
        f.readline()
        setting = yaml.load(f, Loader=yaml.CLoader)
        name = setting.get("PlayerSettings").get("applicationIdentifier").get("Android")
        return name.strip()
    return None

def get_real_package_name(project_dir):
    if len(sys.argv) == 2:
        return sys.argv[1]

    return get_package_name(project_dir)


project_dir = get_project_dir()
assert project_dir is not None, 'Cannot found unity project folder'
script_file = get_script_file(project_dir)
assert script_file is not None, 'Cannot found lua script file'
package_name = get_real_package_name(project_dir)
assert package_name is not None, 'Cannot found package name'

print(script_file)

cmd = f'adb push "{script_file}" /sdcard/Android/data/{package_name}/files/_luarunner.lua'
call(cmd)
cmd = "adb shell input keyevent KEYCODE_F8"
call(cmd)
print("run success")
