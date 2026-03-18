
# Clone TFS Git Repository (Windows PowerShell)

Clone a TFS Git repository on Windows PowerShell. Run these commands exactly. Do not explain anything, just execute.

## Parameters (กรอกข้อมูลด้านล่าง)



TFS_HOST :       siam-tfd-03 
TFS_PORT  :      8080 
TFS_COLLECTION : tfs18/ITGroup_Collection 

Repos list  :
RLOP0001
RLSR250
RLSR61
RLSR267 

BRANCH  :        Master
USERNAME :       
PASSWORD :       
TARGET_FOLDER :  D:\Project\OMNIChannel\Improve Phase 3\Project\Full Assessment\WorkSpace\Ai-Integrated Process\02\test

## Rules
- Shell is Windows PowerShell 5.1. Use `;` to chain commands. NEVER use `&&`.
- Always wrap paths that contain spaces in double quotes.
- Any `@` in PASSWORD must be encoded as `%40` in the git URL.
- NEVER delete or overwrite any existing `.git` folder. It will cause "Access is denied" and waste time.
- Clone into a NEW SUBFOLDER inside TARGET_FOLDER. Use REPO_NAME as the subfolder name.
- If "destination already exists" error occurs, clone into a subfolder instead of trying to fix it.

## Commands to Run (in order)

```powershell
cd "[TARGET_FOLDER]"
```

```powershell
git clone --branch [BRANCH] "http://[USERNAME]:[PASSWORD with @ replaced by %40]@[TFS_HOST]:[TFS_PORT]/[TFS_COLLECTION]/[TFS_PROJECT]/_git/[REPO_NAME]" "[REPO_NAME]"
```

```powershell
cd "[REPO_NAME]" ; git branch ; git log --oneline -3
```

## Expected Output
- `git branch` shows: `* [BRANCH]`
- `git log` shows 3 latest commits

## Error Handling

| Error Message | Solution |
|---------------|----------|
| `Bad hostname` | `@` in password was not encoded as `%40`. Fix the URL. |
| `already exists` | Clone into subfolder: `git clone [url] "[REPO_NAME]"` |
| `Couldn't connect` | Wrong hostname. Run: `git -C "[path_to_any_other_repo]" remote -v` to find the correct TFS hostname. |
| `Access is denied` | Do NOT attempt to delete `.git`. Clone to a different path. |
| `Too many arguments` | Path has spaces without quotes. Add double quotes. |