$projectFile = "src\TestOkur.Infrastructure\TestOkur.Infrastructure.csproj"
$sonarQubeId = "NazmiAltun_TestOkur.Infrastructure"
$github = "NazmiAltun/TestOkur.Infrastructure"
$baseBranch = "master"
$framework = "netcoreapp2.1"
$sonarOrg = "nazmialtun-github"
$sonarToken = "bb48ce419887374c832debe93ea0ee5b9d29914d"

if ($env:APPVEYOR_REPO_NAME -eq $github) {
 
    $prMode = $false;
    $branchMode = $false;
     
    if ($env:APPVEYOR_PULL_REQUEST_NUMBER) { 
        # first check PR as that is on the base branch
        $prMode = $true;
        Write-Output "Sonar: on PR $env:APPVEYOR_PULL_REQUEST_NUMBER"
    }
    elseif ($env:APPVEYOR_REPO_BRANCH -eq $baseBranch) {
        Write-Output "Sonar: on base branch ($baseBranch)"
    }
    else {
        $branchMode = $true;
        Write-Output "Sonar: on branch $env:APPVEYOR_REPO_BRANCH"
    }

    choco install "msbuild-sonarqube-runner" -y

    $sonarUrl = "https://sonarcloud.io"
    $buildVersion = $env:APPVEYOR_BUILD_VERSION

    if ($prMode) {
        $pr = $env:APPVEYOR_PULL_REQUEST_NUMBER
        Write-Output "Sonar: Running Sonar for PR $pr"
        SonarScanner.MSBuild.exe begin /o:"$sonarOrg" /k:"$sonarQubeId" /d:"sonar.host.url=$sonarUrl" /d:"sonar.login=$sonarToken" /v:"$buildVersion" /d:"sonar.cs.opencover.reportsPaths=coverage.xml" /d:"sonar.analysis.mode=preview" /d:"sonar.github.pullRequest=$pr" /d:"sonar.github.repository=$github" /d:"sonar.github.oauth=$env:github_auth_token"
    }
    elseif ($branchMode) {
        $branch = $env:APPVEYOR_REPO_BRANCH;
        Write-Output "Sonar: Running Sonar in branch mode for branch $branch"
        SonarScanner.MSBuild.exe begin /o:"$sonarOrg" /k:"$sonarQubeId" /d:"sonar.host.url=$sonarUrl" /d:"sonar.login=$sonarToken" /v:"$buildVersion" /d:"sonar.cs.opencover.reportsPaths=coverage.xml" /d:"sonar.branch.name=$branch"  
    }
    else {
        Write-Output "Sonar: Running Sonar in non-preview mode, on branch $env:APPVEYOR_REPO_BRANCH"
        SonarScanner.MSBuild.exe begin /o:"$sonarOrg" /k:"$sonarQubeId" /d:"sonar.host.url=$sonarUrl" /d:"sonar.login=$sonarToken" /v:"$buildVersion" /d:"sonar.cs.opencover.reportsPaths=coverage.xml"
    }

    msbuild /t:Rebuild $projectFile /p:targetFrameworks=$framework /verbosity:minimal

    SonarScanner.MSBuild.exe end /d:"sonar.login=$sonarToken"
}
else {
    Write-Output "Sonar: not running as we're on '$env:APPVEYOR_REPO_NAME'"
}