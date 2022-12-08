
<template>
  <div class="greetings">
    <h2 class="green">{{ msg }}</h2>
    <Button type="primary" v-show="userRole != 2" @click="handleSave" style="margin-left: 10px"
      >儲存</Button>
    <Button type="primary" v-show="userRole == 2 || userRole == 4" @click="handleComplete" style="margin-left: 10px"
      >完成</Button>
    <Button type="primary" @click="handleReturn" style="margin-left: 10px"
      >返回主頁面</Button>
  </div>

  <Card style="background: #eee">
    <Form>
        <Row>
          <Col :span="3">
            <Form.Item label="* 模擬的角色" labelAlign="left" />
          </Col>
          <Col :span="21">
            <Form.Item>
              <Select v-model:value="userRole" :options="state.RoleList">
              </Select>
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col :span="3">
            <Form.Item label="* 標題" labelAlign="left" />
          </Col>
          <Col :span="21">
            <Form.Item>
              <Input v-model:value="ticket.title" />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col :span="3">
            <Form.Item label="* 摘要" labelAlign="left" />
          </Col>
          <Col :span="21">
            <Form.Item>
              <Input v-model:value="ticket.summary" />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col :span="3">
            <Form.Item label="* 描述" labelAlign="left" />
          </Col>
          <Col :span="21">
            <Form.Item>
              <Input v-model:value="ticket.description" />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col :span="12">
            <Form.Item label="處理人員" labelAlign="left" :labelCol="queryFormItemStyle1">
              <Input v-model:value="ticket.assignUser" />
            </Form.Item>
          </Col>
          <Col :span="12">
            <Form.Item label="優先級" labelAlign="left" :labelCol="queryFormItemStyle2">
              <Input v-model:value="ticket.priority" />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col :span="12">
            <Form.Item label="類別" labelAlign="left" :labelCol="queryFormItemStyle1">
              <Select v-model:value="ticket.ticketType" :options="state.TypeList">
              </Select>
            </Form.Item>
          </Col>
          <Col :span="12">
            <Form.Item label="狀態" labelAlign="left" :labelCol="queryFormItemStyle2">
              <Select v-model:value="ticket.status" :disabled="true" :options="state.StatusList">           
              </Select>
            </Form.Item>
          </Col>
        </Row>
    </Form>
  </Card>
</template>

<script setup lang="ts">
  import { apiHelper } from '@/services/apiHelper';
  import { onMounted, reactive, ref } from "vue";
  import { useTodoStore } from '@/store';
  import {
    Row,
    Col,
    Form,
    FormItem,
    DatePicker,
    Card,
    Button,
    Input,
    Select,
    SelectOption,
    Table,
  } from 'ant-design-vue';
  import 'ant-design-vue/dist/antd.css';
  import { defHttp } from '../../../services/apiHelper';
  import router from '../../../router';
  import { useRoute } from 'vue-router';
  import { TicketItem } from '/@/models/ticketModel';

  const ticket = reactive({} as TicketItem);
  const msg = ref('問題(編輯/新增)頁面');
  const route = useRoute();
  const id = ref(route.params?.id as string);
  const userRole = ref('');
  const state = ref({
    TypeList: [
      { value: 1, label: 'Bug' },
      { value: 2, label: 'New' },
      { value: 3, label: 'TestCase' },
    ],
    StatusList: [
      { value: 1, label: 'Open' },
      { value: 2, label: 'Finish' },
    ],
    RoleList: [
      { value: 1, label: 'QA' },
      { value: 2, label: 'RD' },
      { value: 3, label: 'PM' },
      { value: 4, label: 'Admin' },
    ],
  });

  onMounted(async () => {
    // msg.value = 'ttt';
    console.log(id.value)
    if(id.value == 'new'){
      ticket.title = 'test';
      ticket.summary = 'test2';
      ticket.description = 'test3';
      ticket.ticketType = 1;
      ticket.status = 1;

      state.value.RoleList =  [
          { value: 1, label: 'QA' },
          { value: 3, label: 'PM' },
          { value: 4, label: 'Admin' },
        ];

    }else{
      await queryTicketById(id.value);
    }
  });
  const queryTicketById = async (id: string) => {
      const obj = (await defHttp.get( { url :`ticket/${id}` }, ''))  as TicketItem;
      ticket.title = obj.title;
      ticket.summary = obj.summary;
      ticket.description = obj.description;
      ticket.ticketType = obj.ticketType;
      ticket.status = obj.status;
      ticket.priority = obj.priority;
      ticket.assignUser = obj.assignUser;
      
  };
  // const GetTicketType = computed((record) =>
  //   record.TicketType == 1 ? 'Bug'
  // );
  const GetTicketType = (record): string => {
    return record.ticketType == 1 ? 'Bug' : '';
  };
  const GetStr = (record): string => {
    return record ? record.title : '';
  };

  const handleSave = async () => {
    let obj = {};
    obj.title = ticket.title;
    obj.summary = ticket.summary;
    obj.description = ticket.description;
    obj.ticketType = Number(ticket.ticketType);
    obj.status = Number(ticket.status);
    obj.priority = ticket.priority;
    obj.assignUser = ticket.assignUser;
    defHttp.setToken(userRole.value);
    try {
      if(id.value == 'new'){        
        let res = await defHttp.post<TicketItem>( { url :'Ticket', data: obj });
        id.value = res.id;
        // const res = await defHttp.post( 'Ticket', obj);
        console.log(res);
        msg.value = 'add successful.';
      }else{
        let res = await defHttp.put<any>( { url :`Ticket/${id.value}`, data: obj });
        console.log(res);
        msg.value = 'update successful.';
      }
    } catch (err) {
      msg.value = `had error.(${err.message})`;
    }
  };
  const handleComplete = async () => {
    defHttp.setToken(userRole.value);
    try {
      if(id.value == 'new'){
        msg.value = 'can not to be complete.';
      }else{
        let res = await defHttp.put<any>( { url :`Ticket/complete/${id.value}` });
        console.log(res);
        msg.value = 'complete successful.';
      }
    } catch (err) {
      msg.value = `had error.(${err.message})`;
    }
  };
  const handleReturn = async () => {
    router.push(`/ticket`);    
  };
</script>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

@media (min-width: 1024px) {
  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style>
