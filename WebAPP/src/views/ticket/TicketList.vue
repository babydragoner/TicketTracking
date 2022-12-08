
<template>
  <div class="greetings">
    <Button type="primary" @click="handleQuery" style="margin-left: 10px"
      >查詢</Button>
    <Button type="primary" @click="handleAdd" style="margin-left: 10px"
      >新增</Button>
  </div>

    <Card>
      <Row>
        <Col :xl="{ span: 24 }" :xxl="{ span: 18 }">
          <Table
            :columns="columns"
            :data-source="ticketsCampaigns"
            rowKey="id"
            bordered
            :loading="idLoading"
            size="small"
          >         
            <template #title="{ record }">
              <a @click="handleDetail(record.id)">{{ GetStr(record) }}</a>
            </template>
            <template #ticketType="{ record }">
              {{ GetTicketType(record) }}
            </template>
            <template #status="{ record }">
              {{ GetStatus(record) }}
            </template>
          </Table>
        </Col>
      </Row>
    </Card>
</template>

<script setup lang="ts">
  import { apiHelper } from '@/services/apiHelper';
  import { TicketItem } from '@/models/ticketModel';
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
  import { defHttp } from '../../services/apiHelper';
  import router from '../../router';

  const idLoading = ref(false);
  const ticketsCampaigns = reactive([] as Array<TicketItem>);
  const columns = [
    {
      title: '名稱',
      dataIndex: 'title',
      slots: { customRender: 'title' },      
    },
    {
      title: '工單類別',
      dataIndex: 'ticketType',
      slots: { customRender: 'ticketType' },
    },
    {
      title: '狀態',
      dataIndex: 'status',
      slots: { customRender: 'status' },
    },
  ];

  onMounted(async () => {
    ticketsCampaigns.splice(0);
  });

  const GetTicketType = (record): string => {
    return record.ticketType == 1 ? 'Bug' : record.ticketType == 2 ? 'New' : record.ticketType == 3 ? 'TestCase' : '';
  };
  const GetStatus = (record): string => {
    return record.status == 1 ? 'Open' : record.status == 2 ? 'Finish' : '';
  };
  const GetStr = (record): string => {
    return record ? record.title : '';
  };

  const handleQuery = async () => {
    idLoading.value = true;
    setTimeout(async () => {
      const obj = (await defHttp.get( { url :'Ticket' }, ''))  as Array<TicketItem>;
      ticketsCampaigns.splice(0);
      ticketsCampaigns.push(...obj);
    idLoading.value = false;
    }, 500);
  };
  const handleAdd = async (id: string) => {
    router.push({ path: `/ticket/info/new` });    
  };
  const handleDetail = async (id: string) => {
    router.push({ path: `/ticket/info/${id}` });    
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
