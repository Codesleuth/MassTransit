// Copyright 2007-2014 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using TestFramework;
    using TestFramework.Messages;


    [TestFixture]
    public class Sending_an_object_to_the_local_bus :
        InMemoryTestFixture
    {
        [Test]
        public async void Should_receive_the_proper_message()
        {
            Task<ConsumeContext<MessageA>> handler = SubscribeToLocalBus<MessageA>();

            ISendEndpoint sendEndpoint = await LocalBusSendEndpoint;

            object message = new MessageA();
            await sendEndpoint.Send(message);

            await handler;
        }

        [Test]
        public async void Should_receive_the_proper_message_type()
        {
            Task<ConsumeContext<MessageA>> handler = SubscribeToLocalBus<MessageA>();

            ISendEndpoint sendEndpoint = await LocalBusSendEndpoint;

            object message = new MessageA();
            await sendEndpoint.Send(message, typeof(MessageA));

            await handler;
        }

        [Test]
        public async void Should_receive_the_interface_of_the_message()
        {
            Task<ConsumeContext<IMessageA>> handler = SubscribeToLocalBus<IMessageA>();

            ISendEndpoint sendEndpoint = await LocalBusSendEndpoint;

            var message = new MessageA();
            await sendEndpoint.Send(message);

            await handler;
        }

        [Test]
        public async void Should_receive_the_interface_proxy()
        {
            Task<ConsumeContext<IMessageA>> handler = SubscribeToLocalBus<IMessageA>();

            ISendEndpoint sendEndpoint = await LocalBusSendEndpoint;

            await sendEndpoint.Send<IMessageA>(new{});

            await handler;
        }

        [Test]
        public async void Should_receive_the_proper_message_as_a()
        {
            Task<ConsumeContext<MessageA>> handler = SubscribeToLocalBus<MessageA>();

            ISendEndpoint sendEndpoint = await LocalBusSendEndpoint;

            var message = new MessageA();
            await sendEndpoint.Send(message);

            await handler;
        }

        [Test]
        public async void Should_receive_the_proper_message_as_a_with_request_id()
        {
            Task<ConsumeContext<MessageA>> handler = SubscribeToLocalBus<MessageA>(x => x.RequestId.HasValue);

            ISendEndpoint sendEndpoint = await LocalBusSendEndpoint;

            var message = new MessageA();
            await sendEndpoint.Send(message, Pipe.New<SendContext>(x => x.Execute(c => c.RequestId = NewId.NextGuid())));

            await handler;
        }
    }
}