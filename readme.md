# pending

https://jsfiddle.net/timdown/UWExN/64/

google translation api




# Purpose

A portal to learning English and Malay language.
Provide collection of essays for reading and learning new vocabulary. The essay will provide context for vocabulary, making learning process more interesting, informative and useful. 

Essays - news, stories ....

# Functions
Each user can build up vocabs from essay or their personal collections

Exercise mode
1. hide chinese meaning
2. Test mode - MCQ
3. Test mode - matching
for those not familir, can mark for review

Tab
Essay
Vocab
Review

# Technology stack
- db : sqlite with Entity Framework ORM
- Microsoft .NET razor pages (server-rendered) + Web API + Alpine.JS (interactive UI)
- css : bootstrap

# IDE
vs code

# Hosting company
freeaspnethosting


# Database Table structure

## Essays

- id
- title
- text
- lang - [zh, en, ms]
- level - [小學、初中、高中]
- source
  
## Vocabs
- id
- essay_id
- en_text
- en_note
- cn_text
- cn_note
- ms_text
- ms_note
