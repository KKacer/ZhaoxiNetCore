using Nest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.Helper;
using Zhaoxi.IocDI.Model;

namespace NUnitTestProject
{
    public class Tests
    {
        private ElasticSearchHelper elasticSearchHelper = new ElasticSearchHelper("http://localhost:9200");
        private ElasticSearchExtendHelper<user> elasticSearchExtend = new ElasticSearchExtendHelper<user>();

        /// <summary>
        /// �����ڹ��캯��
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var isexists = elasticSearchHelper.IsExistsByElasticClientIndex("zhaoxi");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var isexists = elasticSearchHelper.CreateElasticClientIndex("zhaoxi1");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public void Test3()
        {
            var isexists = elasticSearchHelper.DeleteElasticClientIndex("zhaoxi1");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public async Task Test4()
        {
            user user = new user();
            user.Account = "111";
            user.Age = 19;
            user.Name = "z2";
            var re = await elasticSearchExtend.InsertEntityAsync(user);
            Assert.IsTrue(re);
            Assert.Pass();
        }
        [Test]
        public async Task Test5()
        {
            var re = await elasticSearchExtend.FindOne("5mix73IBv-C3Ucz42PR-");
            Assert.IsTrue(re != null);
            Assert.Pass();
        }

        [Test]
        public async Task Test6()
        {
            user user = new user();
            user.Name = "z4yh";
            var re = await elasticSearchExtend.UpdateEntityAsync("5mix73IBv-C3Ucz42PR-", user);
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test7()
        {
            var re = await elasticSearchExtend.FindAll();
            Assert.IsTrue(re.Count > 0);
            Assert.Pass();
        }

        [Test]
        public async Task Test8()
        {
            var re = await elasticSearchExtend.DeleteEntityAsync("5mix73IBv-C3Ucz42PR-");
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test9()
        {
            List<user> list = new List<user>();
            list.Add(new user() { Account = "520", Age = 22, Name = "dd" });
            list.Add(new user() { Account = "521", Age = 20, Name = "mm" });
            var re = await elasticSearchExtend.InsertManyEntityAsync(list);
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test10()
        {
            //���ַ�������
            //.Query(q => q.Match(m => m.Field(f => f.Name).Query("�����ʼǱ�6"))) //���ֶ�ȫ�Ĺؼ��ּ��� ֻҪName�а���ֵ���ɣ����Դ��ִ� 
            //.Query(q => q.MultiMatch(m => m.Fields(fd=>fd.Fields(f=>f.Name,f=>f.OtherInfo)).Query("1��23456789"))) //���ֶ�ȫ�Ĺؼ��ּ��� Name��OtherInfo������ֵ���ɣ����Դ��ִ� 
            //.Analyzer("") // �÷ִʷ����ɲ���Ҫ����Ϊ����Ĳ�ѯ�Դ��ִ�
            //.Query(q => q.Bool(b=>b.Must(m=>m.Term(p=>p.Field(f=>f.Id).Value(4)))))  //����������ϣ��޷ִʣ���һЩ�������Ϳ��ܲ�ѯʧ��
            //.Query(q => q.Range(c => c.Field(f => f.Id).LessThanOrEquals(5).GreaterThanOrEquals(3))) //��Χ��ѯ
            //.Sort(t => t.Ascending(p=>p.Id)) //id����
            //.From(0) //��ҳ �ڼ�����ʼչʾ
            //.Size(3) //��ҳ��ÿҳ��ʾ������

            //�������һ����������������
            //var matchQuery = new List<Func<QueryContainerDescriptor<Computer>, QueryContainer>>
            //{
            //    must => must.Bool(b => b.Must(m => m.Term(p => p.Field(f => f.Id).Value(5)),
            //                                    m => m.Term(p => p.Field(f => f.Name).Value("���ݱʼǱ�1"))
            //                                 )
            //                     ),
            //    range => range.Range(c => c.Field(p => p.Id).LessThanOrEquals(5).GreaterThanOrEquals(3))
            //};
            //var tr = es.Search<Computer>(x=>x.Index("realyuseit").Query(q=>q.Bool(b=>b.Must(matchQuery))))


            //1. 
            //SearchDescriptor<user> searchDescriptor = new SearchDescriptor<user>();
            //searchDescriptor.From(0);
            //searchDescriptor.Size(10);
            //searchDescriptor.Query(q => q.Match(m => m.Field(f => f.Name).Query("mm")));
            //var list = await elasticSearchExtend.FindWhere(searchDescriptor);

            //2.
            var searchRequest = new SearchRequest<user>(Nest.Indices.All)
            {
                From = 0,
                Size = 10,
                Query = new MatchQuery
                {
                    Field = Infer.Field<user>(f => f.Name),
                    Query = "mm"
                }
            };
            var list = await elasticSearchExtend.FindWhere(searchRequest);
            Assert.Pass();
        }
    }
}