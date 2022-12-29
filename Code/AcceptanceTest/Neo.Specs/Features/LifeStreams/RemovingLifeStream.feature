Feature: Removing life stream

In order to registering and tracking life strams online
As me
I want to configure life stream 

Scenario Outline: Life stream gets removed when no any events has registered for it
	Given There is a defined life stream with following properties
		| Title                  | Description |
		| Friendship with Souzan |             |
	When I remove life stream 'Friendship with Souzan'
	Then I can not find life stream 'Friendship with Souzan' with above properties

	#TODO: comming soon
Scenario Outline: Life stream is not allowed to get removed when events has registered for it