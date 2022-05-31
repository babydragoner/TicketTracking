# TicketTracking
 TicketTracking

### Phase I 
- 只有 QA 可以創建錯誤、編輯錯誤和刪除錯誤，錯誤需要摘要字段和描述字段。
- 只有 RD 可以解決錯誤。
### Phase II 
- “PM”，可以創建新的工單類型“功能請求” >> 只有 RD 可以將其標記為已解決。
- QA 才能創建和解決的新工單類型“測試用例”。 它對其他類型的用戶是只讀的。
- “管理員” 可以管理所有東西，包括添加新的 QA、RD 和 PM 用戶。

### Other
- 工單類型:錯誤, 功能請求, 測試用例 三種
- 用戶類型:QA, RD, PM, Admin 四種
- API 文件使用 swagger 會自動解析 XML 註解