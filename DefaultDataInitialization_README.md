# Ĭ�����ݳ�ʼ��ϵͳʵ��˵��

## ����
��ʵ�ֽ����е�������������UserManager�⣩��Ĭ�����ݼ���ͳһ��ȡ��һ�������ķ����У�����Ӧ������ʱ���г�ʼ����

## ��Ҫ���

### 1. DefaultDataService (Services/DefaultDataService.cs)
- **ְ��**: ���й������е�����������Ĭ�����ݳ�ʼ��
- **�ص�**: 
  - ��̬�࣬����ʵ����
  - Ϊÿ���������ṩ�����ĳ�ʼ������
  - �ṩͳһ�ĳ�ʼ����ڵ�

#### ��Ҫ����:
- `InitializeAllDefaultData()` - ��ʼ�����й�������Ĭ������
- `InitializeStudentManagerData()` - ��ʼ��ѧ����������Ĭ������
- `InitializeFileTransferApplicationManagerData()` - ��ʼ������ת�������������Ĭ������

### 2. �޸ĵ��ļ�

#### Services/FileTransferApplicationManager.cs
- **�Ƴ�**: `LoadDefaultData()` ˽�з���
- **�޸�**: ���캯���в��ٵ���Ĭ�����ݼ���

#### App.xaml.cs
- **����**: ��Ӧ������ʱ���� `DefaultDataService.InitializeAllDefaultData()`
- **ʱ��**: ���û���¼״̬���غ󣬴��ڼ���ǰ

## Ĭ����������

### ѧ������ (StudentManager)
- 5��ʾ��ѧ�������������ĸ�����Ϣ���ɼ������ͼ�¼��
- ���ǲ�ͬ�꼶��רҵ��ѧ��
- ���������ļ�״̬����Ϣ

### ����ת���������� (FileTransferApplicationManager)
- 5��ʾ�������¼
- ��ͬ������״̬������Ԥ���С�ת���С�����ɣ�
- ��ʵ�����볡����������Ҫ���������졢����Ǩ�Ƶȣ�

## �������

### 1. ��ע�����
- ���ݳ�ʼ���߼���ҵ���߼�����
- ÿ��������רע����������ݹ�����

### 2. ���й���
- ����Ĭ��������һ���ط�����͹���
- ����ά�����޸�

### 3. ����չ��
- ����������ʱ��ֻ����DefaultDataService����Ӷ�Ӧ�ĳ�ʼ������
- ͳһ�ĳ�ʼ��ģʽ

### 4. �����
- ����ѡ���Եس�ʼ���ض�������������
- ֧���������ú����¼���

## ʹ�÷�ʽ

### Ӧ������ʱ�Զ���ʼ��
```csharp
// ��App.xaml.cs��OnLaunched������
DefaultDataService.InitializeAllDefaultData();
```

### �ֶ���ʼ���ض�������
```csharp
// ֻ��ʼ��ѧ������
DefaultDataService.InitializeStudentManagerData();

// ֻ��ʼ������ת����������
DefaultDataService.InitializeFileTransferApplicationManagerData();
```

## ע������
- UserManager���ų���Ĭ�����ݳ�ʼ��֮�⣬��Ϊ�û�����ͨ��������֤ϵͳ
- ��ʼ��������������ݣ�Ȼ�����Ĭ������
- ȷ����Ӧ������ʱ���ã��������ݾ�������