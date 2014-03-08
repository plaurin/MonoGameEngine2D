param($installPath, $toolsPath, $package, $project)

# $project.ProjectItems | select-Object Name | Write-Host
$item = $project.ProjectItems | where-object {$_.Name -eq "Content"} 

# $item.ProjectItems | select-Object Name | Write-Host
$item = $item.ProjectItems | where-object {$_.Name -eq "SpriteFont.xnb"} 

$item.Properties.Item("BuildAction").Value = [int]2