param($installPath, $toolsPath, $package, $project)

# SpriteFont.xnb build action
$item = $project.ProjectItems | where-object {$_.Name -eq "Content"} 
$item = $item.ProjectItems | where-object {$_.Name -eq "SpriteFont.xnb"} 
$item.Properties.Item("BuildAction").Value = [int]2

# Content Sync
$pre = $project.Properties | where-object {$_.Name -eq "PreBuildEvent"}
$pre.Value = 'REM Replace WindowsProject by your actual project name: "$(ProjectDir)\SyncTool\ContentSync.exe" "$(ProjectPath)" "$(SolutionDir)\WindowsProject\WindowsProjects.csproj"'