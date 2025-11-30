# 📝 PrintMenuCleaner 项目文档

------

## 1️⃣ 项目概述

**项目名称**：PrintMenuCleaner
 **语言/框架**：C# / .NET 9 / WinForms
 **类型**：单文件 GUI + CLI 工具
 **功能**：扫描、删除 Windows 注册表右键“打印(Print/PrintTo)”菜单，支持备份与恢复

<img src="image\README\1764476466326.png" style="zoom:50%;" />

**主要特点**：

- 扫描注册表中所有带 `print` / `printto` 的右键菜单
- CLI + GUI 双模式
- 自动生成备份 `.reg` 文件
- 支持恢复删除的注册表项
- 日志记录操作
- 单文件发布，免安装 .NET SDK/Runtime

------

## 2️⃣ 项目目录结构

```
PrintMenuCleaner/
│  PrintMenuCleaner.csproj
│  Program.cs            ← 程序入口，判断 CLI / GUI
│  Cleaner.cs            ← 核心逻辑：扫描 / 删除 / 备份 / 恢复 / 日志
│  MainForm.cs           ← GUI 主窗体逻辑
│  MainForm.Designer.cs  ← GUI 界面设计
│  bin/                  ← 编译输出目录
│  obj/                  ← 临时构建文件
```

------

## 3️⃣ 核心功能说明

### 3.1 扫描注册表

- 扫描 `HKCR` 下所有子项
- 找出 `shell\print` 和 `shell\printto` 子键
- 支持并行扫描，自动过滤系统关键项（如 CLSID、AppID 等）
- GUI 显示扫描结果，可勾选删除

### 3.2 删除注册表项

- GUI：勾选要删除的项，点击“删除”
- CLI：`--no-backup` 参数直接删除
- 删除前可生成 `.reg` 备份，保证可恢复

### 3.3 备份注册表

- GUI：点击“备份”生成 `.reg` 文件
- CLI：默认删除时生成备份（可通过 `--no-backup` 关闭）
- 备份路径默认与 exe 同目录
- 支持 Unicode 编码，保证中文路径/键值正确

### 3.4 恢复注册表

- GUI：点击“恢复”选择 `.reg` 文件
- CLI：`--restore <file>`
- 调用 `regedit.exe /s` 导入注册表

### 3.5 日志记录

- 所有扫描、删除、备份、恢复操作记录到 `PrintMenuLog_yyyyMMdd_HHmmss.txt`
- 默认保存在 exe 同目录
- GUI 界面实时显示日志

------

## 4️⃣ CLI 参数说明

| 参数                  | 说明                                  |
| --------------------- | ------------------------------------- |
| `--scan-only`         | 扫描注册表并输出所有 print/printto 项 |
| `--no-backup`         | 删除所有 print/printto 项，不生成备份 |
| `--restore <regfile>` | 从指定 `.reg` 文件恢复注册表项        |
| 无参数                | 默认启动 GUI                          |

------

## 5️⃣ GUI 使用说明

### 主界面组件

| 组件         | 说明                                        |
| ------------ | ------------------------------------------- |
| DataGridView | 显示扫描到的 print/printto 注册表项，可勾选 |
| 扫描按钮     | 扫描注册表并刷新列表                        |
| 备份按钮     | 生成当前列表的 `.reg` 备份文件              |
| 删除按钮     | 删除勾选的注册表项（可自动备份）            |
| 恢复按钮     | 从指定 `.reg` 文件恢复注册表项              |
| 日志框       | 实时显示操作日志                            |

### 使用流程

1. 点击 **扫描** → 列出注册表中所有 print/printto 菜单
2. 可勾选需要删除的项
3. 点击 **备份**（可选） → 保存当前列表备份
4. 点击 **删除** → 删除勾选项
5. 点击 **恢复** → 可选择历史备份恢复

------

## 6️⃣ 单文件 exe 发布说明

### 6.1 csproj 配置

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net9.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
    <Nullable>enable</Nullable>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <IncludeAllContentForSelfExtract>true</IncludeAllContentForSelfExtract>
    <PublishTrimmed>false</PublishTrimmed>
    <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
  </PropertyGroup>
</Project>
```

### 6.2 发布命令

```bash
dotnet publish -c Release
```

- 发布路径：

```
PrintMenuCleaner\bin\Release\net9.0-windows\win-x64\publish\
```

- 生成 `PrintMenuCleaner.exe`，双击即可运行 GUI 或在命令行使用 CLI 参数

### 6.3 注意事项

- 发布后的 exe 包含 .NET 运行时，无需额外安装
- 默认日志和备份文件生成在 exe 同目录
- 支持 64 位 Windows，如需 32 位，将 `RuntimeIdentifier` 改为 `win-x86`

------

## 7️⃣ 安全提示

- 修改注册表有风险，删除前务必备份
- 建议运行程序 **以管理员身份**，保证删除/恢复权限
- 备份文件可长期保存，用于恢复意外操作

------

## 8️⃣ 开发注意事项

- `Cleaner.FoundKeys` 为只读属性，外部修改请使用 `Cleaner.SetFoundKeys(List<string>)`
- 日志文件线程安全，支持并行扫描
- GUI 采用 WinForms，可进一步优化为 WPF 或现代界面

------

## 9️⃣ 更新记录

| 版本 | 功能更新                                                |
| ---- | ------------------------------------------------------- |
| v1.0 | 初始版本，CLI 扫描/删除/备份/恢复功能                   |
| v1.1 | GUI 界面，列表勾选删除，日志显示                        |
| v1.2 | 单文件 exe 发布，自动备份路径和日志统一到 exe 根目录    |
| v1.3 | CLI 参数优化：`--scan-only`、`--no-backup`、`--restore` |

------

## 10️⃣ 使用示例

### GUI 模式

双击 `PrintMenuCleaner.exe` → 打开窗口 → 扫描 → 勾选删除 → 备份 → 删除

### CLI 模式

```bash
# 扫描并输出结果
PrintMenuCleaner.exe --scan-only

# 删除所有 print/printto，不生成备份
PrintMenuCleaner.exe --no-backup

# 恢复注册表
PrintMenuCleaner.exe --restore PrintMenuBackup_20251130_120000.reg
```

