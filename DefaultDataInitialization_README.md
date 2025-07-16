# 默认数据初始化系统实现说明

## 概述
本实现将所有单例管理器（除UserManager外）的默认数据加载统一抽取到一个独立的服务中，并在应用启动时进行初始化。

## 主要组件

### 1. DefaultDataService (Services/DefaultDataService.cs)
- **职责**: 集中管理所有单例管理器的默认数据初始化
- **特点**: 
  - 静态类，无需实例化
  - 为每个管理器提供独立的初始化方法
  - 提供统一的初始化入口点

#### 主要方法:
- `InitializeAllDefaultData()` - 初始化所有管理器的默认数据
- `InitializeStudentManagerData()` - 初始化学生管理器的默认数据
- `InitializeFileTransferApplicationManagerData()` - 初始化档案转递申请管理器的默认数据

### 2. 修改的文件

#### Services/FileTransferApplicationManager.cs
- **移除**: `LoadDefaultData()` 私有方法
- **修改**: 构造函数中不再调用默认数据加载

#### App.xaml.cs
- **新增**: 在应用启动时调用 `DefaultDataService.InitializeAllDefaultData()`
- **时机**: 在用户登录状态加载后，窗口激活前

## 默认数据内容

### 学生数据 (StudentManager)
- 5个示例学生，包含完整的个人信息、成绩、奖惩记录等
- 涵盖不同年级和专业的学生
- 包含档案文件状态等信息

### 档案转递申请数据 (FileTransferApplicationManager)
- 5个示例申请记录
- 不同的申请状态（档案预备中、转递中、已完成）
- 真实的申请场景（工作需要、继续深造、户口迁移等）

## 设计优势

### 1. 关注点分离
- 数据初始化逻辑与业务逻辑分离
- 每个管理器专注于自身的数据管理功能

### 2. 集中管理
- 所有默认数据在一个地方定义和管理
- 易于维护和修改

### 3. 可扩展性
- 新增管理器时，只需在DefaultDataService中添加对应的初始化方法
- 统一的初始化模式

### 4. 灵活性
- 可以选择性地初始化特定管理器的数据
- 支持数据重置和重新加载

## 使用方式

### 应用启动时自动初始化
```csharp
// 在App.xaml.cs的OnLaunched方法中
DefaultDataService.InitializeAllDefaultData();
```

### 手动初始化特定管理器
```csharp
// 只初始化学生数据
DefaultDataService.InitializeStudentManagerData();

// 只初始化档案转递申请数据
DefaultDataService.InitializeFileTransferApplicationManagerData();
```

## 注意事项
- UserManager被排除在默认数据初始化之外，因为用户数据通常来自认证系统
- 初始化会清空现有数据，然后加载默认数据
- 确保在应用启动时调用，避免数据竞争问题