import re

f = r'E:\Work\ESB\AiStandart\Ai-Integrated Process\CodingStandard\Coding_Standard_Criteria.md'
c = open(f, 'r', encoding='utf-8').read()
lines = c.split('\n')
print('Total lines:', len(lines))
print()

# Issue 1: CorrelationIdMiddleware
print('=== Issue #1: CorrelationIdMiddleware ===')
for i, l in enumerate(lines):
    if 'CorrelationIdMiddleware' in l:
        print(f'  Line {i+1}: {l.strip()[:120]}')

# Issue 2: Route pattern
print('\n=== Issue #2: Route [action] ===')
for i, l in enumerate(lines):
    if '[action]' in l:
        print(f'  Line {i+1}: {l.strip()[:120]}')

# Issue 3: xUnit Default
print('\n=== Issue #3: xUnit Default ===')
for i, l in enumerate(lines):
    if 'xUnit' in l and 'Default' in l:
        print(f'  Line {i+1}: {l.strip()[:120]}')

# Issue 4: Count all unique criteria IDs
print('\n=== Issue #4: Criteria count ===')
criteria = re.findall(r'^\| (\d+\.\d+) \|', c, re.MULTILINE)
unique = sorted(set(criteria), key=lambda x: (int(x.split('.')[0]), int(x.split('.')[1])))
sections = {}
for cid in unique:
    sec = int(cid.split('.')[0])
    sections.setdefault(sec, []).append(cid)

doc_claims = {1:7, 2:53, 3:30, 4:8, 5:7, 6:6, 7:6, 8:7, 9:6, 10:5, 11:10,
              12:15, 13:12, 14:10, 15:10, 16:10, 17:10, 18:10, 19:8, 20:6, 21:4, 22:8, 23:6, 24:8, 25:5, 26:7}
total_doc = 0
total_actual = 0
for s in sorted(doc_claims):
    d = doc_claims[s]
    a = len(sections.get(s, []))
    total_doc += d
    total_actual += a
    if d != a:
        ids = sections.get(s, [])
        print(f'  Sec {s:2d}: doc={d:3d} actual={a:3d} diff={a-d:+d}  IDs: {ids}')
e_criteria = re.findall(r'^\| E\.\d+ \|', c, re.MULTILINE)
print(f'  ADO.NET E.x: {len(e_criteria)}')
print(f'  Sum doc claims: {total_doc} + ADO {len(e_criteria)} = {total_doc + len(e_criteria)}')
print(f'  Sum actual unique: {total_actual} + ADO {len(e_criteria)} = {total_actual + len(e_criteria)}')
print(f'  Doc says grand total: 279')

# Issue 5: s_ prefix
print('\n=== Issue #5: s_ prefix ===')
for i, l in enumerate(lines):
    if 's_' in l and 'prefix' in l.lower():
        print(f'  Line {i+1}: {l.strip()[:120]}')

# Issue 6: end_of_line
print('\n=== Issue #6: end_of_line ===')
for i, l in enumerate(lines):
    if 'end_of_line' in l:
        print(f'  Line {i+1}: {l.strip()[:120]}')
