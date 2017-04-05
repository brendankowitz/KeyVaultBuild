# KeyVaultBuild

## Getting started

Install-Package KeyVaultBuildTask

### Converting config files to templates

A CLI tool is provided to help convert your existing files into .keyvault.template files that the build task will process and substitute during build time.

An example of how to do this is as follows:
```
KeyVaultBuild.Cli.exe template -templateFile "c:\full\path\to\App.config"
                               -aadDirectory <GUID>
                               -vault keyvaultName
```

### Keytemplate syntax

Below is a simple App.keyvault.template file showing keys that will be substituted at build time.

In the following key `#{keyvault:keyvaultbuild:appsetting-key1password}`, the first part is a marker `#{keyvault:` then is the name of the vault `keyvaultbuild` and following this is the name of the key that will be fetched `app-settingkey1password`.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="key1password" value="#{keyvault:keyvaultbuild:appsetting-key1password}" />
    <add key="key2secret" value="#{keyvault:keyvaultbuild:appsetting-key2secret}" />
    <add key="key3" value="test3" />
  </appSettings>
  <connectionStrings>
    <add connectionString="blah" name="hey" conectionString="#{keyvault:keyvaultbuild:connectionstring-hey}" />
  </connectionStrings>
</configuration>
```

### Using the build task to pull values from keyvault

```
<KeyVaultBuild_VaultAliases Condition=" '$(KeyVaultBuild_VaultAliases)' == '' "></KeyVaultBuild_VaultAliases>
<KeyVaultBuild_ClientId Condition=" '$(KeyVaultBuild_ClientId)' == '' "></KeyVaultBuild_ClientId>
<KeyVaultBuild_Secret Condition=" '$(KeyVaultBuild_Secret)' == '' "></KeyVaultBuild_Secret>
<KeyVaultBuild_DirectoryId Condition=" '$(KeyVaultBuild_DirectoryId)' == '' "></KeyVaultBuild_DirectoryId>
```
