# TicketRush — Phases and Task Roadmap

دي خريطة مختصرة للمراحل والمهام، مستخرجة من الـbacklog في `TicketRush_Full_Project_Plan.md`. عناوين المهام محفوظة زي ما وردت في الخطة، والوصف العربي يوضح هنشتغل على إيه.

## الصورة العامة

- عدد المراحل: **14 phase** من V0 إلى V13.
- عدد صفوف المهام الفعلي في الـbacklog: **102 task**.
- السطر التمهيدي في الخطة يقول إن الإجمالي 95 مهمة، لكن عدّ الصفوف ينتج 102 عند احتساب `D0-02A`، و101 لو استبعدناها. ده اختلاف حسابي في المستند الأصلي؛ الملف ده يسجل العد الفعلي من الصفوف ولا يغيّر الخطة المصدر.
- كل مهمة ليها تحقق، وكل phase ليها stage gate. ما ننتقلش للمرحلة التالية قبل استيفاء بوابة المرحلة وتوثيق النتيجة.
- متابعة التنفيذ الحالي موجودة في `PROJECT_STATE.md`، وسجل الشغل في `WORK_LOG.md`.

## V0 — Walking skeleton — 14 tasks

**الهدف:** تأسيس الاتفاقات والـsolution وقاعدة البيانات والحد الأدنى من رحلة المستخدم، مع CI ونظافة المستودع.

- **D0-01 — Repo conventions, `AGENTS.md`, project state/log, templates:** وضع قواعد المستودع وملفات متابعة المهام وقوالب Issue وPull Request. **تمت.**
- **D0-02 — Solution and project skeleton:** إنشاء Solution ومشاريع Domain وApplication وInfrastructure وAPI وWorker والاختبارات، وضبط اتجاه المراجع. **تمت.**
- **D0-02A — Day-one CI + public-repo hygiene:** إعداد GitHub Actions للبناء والـformat والاختبارات، وإضافة `.gitignore` وفحص الأسرار وإعدادات نموذجية آمنة.
- **D0-03 — ADR-0001/0002:** تسجيل قرارات architecture وحدود المشاريع، وUUID والوقت والمال وطريقة تخزين enums.
- **D0-04 — Identity/venue/catalog domain types:** تعريف أنواع وقواعد المستخدمين والأدوار والأماكن والفعاليات والعروض.
- **D0-05 — Inventory/hold/order/payment/ticket domain types:** تعريف حالات المخزون والحجز المؤقت والطلب والدفع والتذكرة وانتقالاتها القانونية.
- **D0-06 — EF mappings and DbContext:** ربط الكيانات بقاعدة البيانات وتعريف المفاتيح والعلاقات والقيود بوضوح.
- **D0-07 — Initial migration + Compose PostgreSQL:** إنشاء أول migration وتجهيز PostgreSQL محليًا بـDocker Compose والتحقق من قاعدة فارغة.
- **D0-08 — Deterministic seed data:** إضافة بيانات seed ثابتة يمكن تشغيلها أكثر من مرة من غير تكرار أو تغيير غير متوقع.
- **D0-09 — Register/login/access JWT:** تنفيذ تسجيل المستخدم وتسجيل الدخول وJWT وتخزين password hash والأدوار الأساسية.
- **D0-10 — Public catalog and cursor basics:** عرض الفعاليات والعروض وخريطة المقاعد مع pagination محدودة ومستقرة.
- **D0-11 — Organizer CRUD/layout generator scaffold:** تمكين المنظم من إعداد venue وevent وshow وprice tiers وتوليد layout ضمن حدود.
- **D0-12 — Naive booking test-only flag + thin JS demo:** عرض تجربة حجز مبسطة محليًا/للاختبار فقط، مع منع تفعيل مسارها في التشغيل العادي.
- **D0-13 — Breadth scaffold: event lifecycle + admin basics + notification read model:** إضافة انتقالات نشر وإلغاء الفعالية، أساسيات الإدارة والتدقيق، وقراءة الإشعارات.

**بوابة V0:** تشغيل محلي موثق، بيانات catalog جاهزة، أساس auth والمنظم موجود، وتجربة السباق الساذجة معزولة عن الاستخدام العادي.

## V1 — Measurement — 7 tasks

**الهدف:** تجهيز القياس قبل ادعاء أي تحسن أو سعة.

- **V1-01 — Structured logs and correlation IDs:** ربط سجلات الطلبات والمهام الخلفية بمعرّف تتبع مع حجب الأسرار.
- **V1-02 — OpenTelemetry traces:** تتبع رحلة الطلب من API إلى قاعدة البيانات في بيئة محلية.
- **V1-03 — Prometheus metrics:** عرض أرقام الطلبات والأخطاء والـlatency واعتماديات النظام.
- **V1-04 — Basic Grafana dashboard:** لوحة أولية للـRPS وp95 والأخطاء واتصالات قاعدة البيانات.
- **V1-05 — Deterministic dataset generator:** إنشاء datasets قابلة لإعادة الإنتاج مع حفظ seed والإصدارات والأعداد.
- **V1-06 — k6 scenarios/results layout:** تجهيز سيناريوهات حمل أولية للـcatalog وseat map وحفظ نتائجها الخام.
- **V1-07 — Invariant checker v1:** فحص تناقضات المخزون والحجوزات والمبالغ في البيانات، مع تجربة بيانات سليمة وفاسدة.

**بوابة V1:** القياس والـinvariant checker قابلان للتكرار، وبيانات البيئة والنتائج محفوظة.

## V2 — Concurrency correctness — 8 tasks

**الهدف:** إثبات أن حجز المقاعد والتذاكر لا يبيع المخزون مرتين تحت الطلبات المتزامنة.

- **V2-01 — Demonstrate naive reserved-seat race:** إعادة إنتاج سباق الحجز الساذج في test محلي.
- **V2-02 — Atomic conditional reserved-seat claim:** جعل قاعدة البيانات تمنح المقعد لطلب واحد ذريًا وترفض الخاسر بتعارض.
- **V2-03 — DB constraints for show-seat identity:** فرض عدم تكرار صف المقعد نفسه داخل العرض الواحد بقيد قاعدة بيانات.
- **V2-04 — Optimistic-concurrency variant:** تجربة version-based updates وقياس التعارضات وإعادة المحاولة.
- **V2-05 — Pessimistic-lock variant:** تجربة أقفال الصفوف وفهم الانتظار والـtimeouts.
- **V2-06 — Choose seat strategy + ADR-0004:** اختيار استراتيجية المقاعد بناءً على صحة السلوك والقياسات وتوثيق القرار.
- **V2-07 — Atomic GA decrement and safe release:** خصم GA بعملية ذرية تمنع السالب وتعيد الكمية مرة واحدة فقط.
- **V2-08 — Regression suite and retire naive path:** تثبيت اختبارات منع البيع الزائد وإخراج المسار الساذج من التشغيل الطبيعي.

**بوابة V2:** اختبارات PostgreSQL المتزامنة تثبت عدم تكرار حجز المقعد وعدم نزول GA تحت الصفر.

## V3 — Holds, expiry, basic realtime and breadth — 11 tasks

**الهدف:** بناء الحجز المؤقت وانتهائه، تحديث خريطة المقاعد، وإكمال بعض تدفقات الدخول والتقارير.

- **V3-01 — Expiry semantics + ADR-0006:** تحديد الساعة المرجعية وسياسة السباق بين انتهاء الحجز وإتمام الشراء.
- **V3-02 — Atomic seat/GA hold creation:** إنشاء hold لكل المقاعد/الكمية أو التراجع الكامل عن العملية.
- **V3-03 — Release/extend:** تمكين المالك من تحرير أو تمديد hold ضمن حدود معروفة وبشكل آمن عند التكرار.
- **V3-04 — Expiry worker:** تشغيل عامل ينظف الحجوزات المنتهية بدفعات، من غير تحرير المخزون مرتين.
- **V3-05 — Expiry-vs-checkout race:** اختبار فوز checkout أو expiry وحده عند التنافس.
- **V3-06 — Hold dashboard/runbook:** إظهار عدد الحجوزات النشطة وتأخر التنظيف وخطوات التعامل معه.
- **V3-07 — Basic SignalR hub + show groups:** إنشاء مجموعات تحديث حسب العرض والتحقق من صلاحية الانضمام لها.
- **V3-08 — Publish seat updates after DB commit:** إرسال تحديث المقعد بعد نجاح commit فقط، ومن غير بيانات شخصية.
- **V3-09 — Seat-map UI live updates + resync:** تحديث الواجهة لحظيًا ثم إعادة جلب الحالة الرسمية بعد انقطاع الاتصال.
- **V3-10 — Check-in, attendee list and sales report breadth:** إضافة فحص التذكرة مرة واحدة وقوائم وتقارير منظّم محدودة الصلاحيات.
- **V3-11 — Hold countdown UI:** عرض عداد يعتمد على `expiresAt` الذي أرسله الخادم، وليس ساعة المتصفح وحدها.

**بوابة V3:** hold وexpiry آمنان، الاسترداد للمخزون يحدث مرة واحدة، والتحديث اللحظي يعيد المزامنة من API.

## V4 — Idempotency, payment, ticket issuance and UI — 10 tasks

**الهدف:** حماية checkout من تكرار الطلبات، ومحاكاة الدفع وإصدار التذاكر.

- **V4-01 — Idempotency acquisition + fingerprint:** حجز مفتاح idempotency في قاعدة البيانات ومقارنة بصمة الطلب.
- **V4-02 — Checkout transaction / snapshots:** إنشاء order واحد من hold مع نسخ السعر والمجموع داخل transaction.
- **V4-03 — Deterministic payment simulator:** محاكاة نجاح ورفض ونتيجة مجهولة بشكل متكرر في بيئة محلية/اختبار فقط.
- **V4-04 — Payment idempotency/state transitions:** ضبط تكرار محاولات الدفع والانتقالات القانونية بين حالاتها.
- **V4-05 — Unknown reconciliation:** إبقاء timeout الغامض `Unknown` حتى تتوفر نتيجة تحقق.
- **V4-06 — Cancellation/refund inventory rules:** تحديد متى يعود المخزون عند الإلغاء أو refund ومنع الإرجاع المكرر.
- **V4-07 — EXP-04 + ADRs:** توثيق تجارب idempotency والدفع الغامض وسياسات replay والاحتفاظ.
- **V4-08 — Synchronous ticket issuance + QR endpoint (EXP-08 baseline):** إصدار التذاكر بعد دفع مؤكد، وحفظ baseline قبل التحويل للمعالجة async.
- **V4-09 — Checkout + payment outcome UI:** عرض نجاح الدفع أو رفضه أو حالة الانتظار بوضوح مع مفاتيح idempotency.
- **V4-10 — My tickets UI + ticket detail/QR:** تمكين المستخدم من مشاهدة تذاكره وQR الخاص به وحالتها.

**بوابة V4:** retries لا تنتج طلبات أو تذاكر مكررة، ولا تصدر تذكرة لدفعة مرفوضة أو مجهولة.

## V5 — DB tuning and caching — 7 tasks

**الهدف:** قياس الاستعلامات وتحسينها، ثم تقرير هل cache مفيد وآمن للعرض.

- **V5-01 — Catalog/seat-map baseline:** تكرار baseline لنفس البيانات والحمل ثلاث مرات على الأقل.
- **V5-02 — Query projections/index candidates:** مراجعة query plans وتجربة projections وindexes المناسبة.
- **V5-03 — Tuned DB-only remeasurement:** إعادة القياس بعد tuning ومقارنة التحسن والتفاوت.
- **V5-04 — Catalog cache-aside flag:** إضافة cache اختياري لقوائم catalog مع إمكانية تعطيله.
- **V5-05 — Seat-map/availability display cache only if useful:** اختبار cache للعرض فقط والتأكد أن قرار الحجز يستعلم قاعدة البيانات.
- **V5-06 — Invalidation/stampede protection if justified:** معالجة stale cache والـstampede فقط إذا القياس يبررها.
- **V5-07 — EXP-05 + ADR-0009:** تسجيل نتيجة مقارنة DB/cache، حتى لو كانت النتيجة إزالة cache.

**بوابة V5:** قرار cache مدعوم بنتائج قابلة للمقارنة، وفشل Redis أو stale display لا يسمح ببيع زائد.

## V6 — PostgreSQL vs Redis holds — 4 tasks

**الهدف:** تجربة Redis كمساعد للحجز مع إبقاء PostgreSQL الحكم النهائي.

- **V6-01 — RFC and safety boundaries:** توثيق حدود السلامة ونوافذ الفشل قبل prototype.
- **V6-02 — Redis `SET NX` + TTL prototype behind flag:** بناء تجربة معزولة لا تتجاوز حارس قاعدة البيانات.
- **V6-03 — DB-only vs Redis-assisted benchmark:** مقارنة النسختين تحت حمل وبيئة متماثلين مع فحوص الصحة.
- **V6-04 — ADR-0010 decision:** تقرير الاحتفاظ بالتجربة أو اختيار PostgreSQL وحده بناءً على الدليل.

**بوابة V6:** قرار موثق بنتيجة التجربة، مع بقاء PostgreSQL المصدر النهائي للمخزون.

## V7 — Rate limiting — 6 tasks

**الهدف:** مقارنة سياسات تحديد الطلبات وتطبيق السياسة المناسبة عبر أكثر من API instance.

- **V7-01 — Policy config + local limiter:** إعداد سياسات مختلفة للمسارات وإرجاع `429` موحد.
- **V7-02 — Spike fixed/sliding/token bucket:** تجربة الخوارزميات وملاحظة burst والحدود.
- **V7-03 — Compare local algorithms:** مقارنة الخوارزميات وتأثيرها على المستخدم الطبيعي تحت حمل.
- **V7-04 — Two API instances + proxy:** إثبات كيف تتضاعف حدود الـlimiter المحلي بين instances.
- **V7-05 — Redis distributed limiter:** تنسيق العدادات ذريًا عبر Redis مع تحديد سلوك الانقطاع.
- **V7-06 — Bot simulator + EXP-07:** محاكاة bot traffic وقياس `429` وأثره على قاعدة البيانات.

**بوابة V7:** سياسات موزعة ومختبرة، وسلوك outage موثق لكل مجموعة مسارات.

## V8 — RabbitMQ/outbox/inbox — 9 tasks

**الهدف:** إضافة معالجة خلفية قابلة للاستعادة والتعامل الصحيح مع تكرار الرسائل.

- **V8-01 — Queue topology/message envelope:** تحديد queues وrouting keys وشكل الرسالة وإصداراتها.
- **V8-02 — Transactional outbox writes:** حفظ التغيير التجاري ونيّة نشر الرسالة في transaction واحدة.
- **V8-03 — Bounded publisher:** نشر دفعات برسائل قابلة لإعادة المحاولة مع افتراض احتمال التكرار.
- **V8-04 — Consumer inbox:** تسجيل الرسائل المعالجة لمنع تكرار الأثر الدائم.
- **V8-05 — Async ticket issuance/notification flow:** نقل إصدار التذاكر والإشعارات إلى Worker ومقارنتهما بالـbaseline المتزامن.
- **V8-06 — Retry/backoff/DLQ/poison handling:** وضع retries محدودة وتحويل الرسائل غير القابلة للمعالجة إلى DLQ.
- **V8-07 — Admin DLQ/replay + audit:** عرض وإعادة تشغيل الرسائل بصلاحية Admin وسبب مسجل.
- **V8-08 — ACK-after-commit, prefetch, graceful shutdown:** تأكيد الرسالة بعد commit وضبط العمل الجاري وقت الإيقاف.
- **V8-09 — EXP-08 + runbook:** قياس latency وتأخر queue والتكرار وتوثيق الاستعادة.

**بوابة V8:** crash/retry/duplicate لا يضيّع الشغل ولا يكرر الأثر التجاري، وDLQ قابل للمراقبة والاستعادة.

## V9 — Waiting room — 7 tasks

**الهدف:** تنظيم دخول المستخدمين وقت الإقبال، من غير أن يتجاوز admission حارس المخزون.

- **V9-01 — RFC for fairness/token policy:** تعريف ترتيب الصف والـtoken والانتهاء وإعادة الاتصال وسياسة الفشل.
- **V9-02 — Idempotent join/status/leave:** ضمان عضوية واحدة لكل مستخدم وعرض حالته وإمكانية الخروج.
- **V9-03 — Atomic admission worker:** إدخال دفعات محدودة ومنع قبول نفس المستخدم أكثر من مرة.
- **V9-04 — Token validation in hold path:** ربط token بالمستخدم والعرض والجلسة والوقت، ثم التحقق منه عند إنشاء hold.
- **V9-05 — UI queue state:** عرض حالة queued/admitted/expired واستعادة الحالة بعد reconnect.
- **V9-06 — Surge/fairness/failure tests:** قياس الاندفاع والعدالة وفشل Redis بحسب قدرة الجهاز.
- **V9-07 — EXP-09 + cut-line tag:** إصدار تقرير تجربة الانتظار والتحقق من عدم خرق المخزون.

**بوابة V9:** تجربة waiting room قابلة للعرض ونتائج العدالة والفشل موثقة، مع صفر خرق لمخزون الاختبار.

## V10 — SignalR scale-out — 2 tasks

**الهدف:** قياس push مقابل polling وإضافة backplane فقط إذا احتجنا عدة instances.

- **V10-04 — Polling vs push comparison:** مقارنة حجم الطلبات وتأخر التحديث واستهلاك الموارد.
- **V10-05 — Redis backplane if needed:** مشاركة رسائل SignalR بين instances عند إثبات الحاجة.

**بوابة V10:** إثبات resync بعد الانقطاع؛ قرار الـbackplane مبني على تجربة متعددة instances.

## V11 — Scale-out/database — 5 tasks

**الهدف:** تشخيص اختناق قاعدة البيانات والاتصالات قبل تغيير إعدادات التوسع.

- **V11-01 — Mixed workload + proxy + API replicas:** تشغيل خليط معروف من القراءة والحجز والـcheckout على عدة instances.
- **V11-02 — Measure pool wait/DB lock waits:** فصل انتظار connection عن وقت query وأقفال قاعدة البيانات.
- **V11-03 — Tune pool; optional PgBouncer:** مقارنة حجم pool الحالي بتغييرات مدروسة أو PgBouncer اختياري.
- **V11-04 — Review indexes under mixed load:** قياس أثر indexes على القراءة والكتابة تحت حمل مختلط.
- **V11-05 — Decide read replica/partitioning:** تقرير الحاجة إلى replica أو partitioning وفق القياسات.

**بوابة V11:** تحديد bottleneck بالدليل؛ لا replica أو partitioning لمجرد أنها أدوات متاحة.

## V12 — Security hardening — 6 tasks

**الهدف:** اختبار الصلاحيات والـtokens وحدود المدخلات وحماية الأسرار.

- **V12-01 — Threat model + OWASP mapping:** توثيق التهديدات والتخفيفات والمخاطر المتبقية.
- **V12-02 — Refresh rotation/reuse/logout:** اختبار تدوير refresh token ورفض القديم والتعامل مع إعادة الاستخدام.
- **V12-03 — IDOR/object/function authorization:** إثبات منع مستخدم من الوصول لموارد شخص أو منظم آخر.
- **V12-04 — Input bounds/abuse controls:** وضع حدود للأعداد والأحجام والفترات الزمنية واختبار تجاوزها.
- **V12-05 — Secrets/log redaction/dependency audit:** فحص المستودع والسجلات والحزم وإزالة تسريب الأسرار.
- **V12-06 — Security report/tag:** تلخيص النتائج والمخاطر المتبقية وإغلاق بوابة المرحلة.

**بوابة V12:** فحوص الأمن والصلاحيات والتدوير والحجب ناجحة، والمخاطر المتبقية موثقة.

## V13 — Delivery — 6 tasks

**الهدف:** جعل المشروع سهل التشغيل والمراجعة وعرض نتائجه بصدق.

- **V13-01 — Extend day-one CI with integration tests, analyzers, container build and security scan:** توسيع CI ليشمل الاختبارات والتحليل وبناء الحاويات والفحص الأمني.
- **V13-02 — Container build + vulnerability scan:** بناء قابل للتكرار وفحص صورة الحاوية وتوثيق النتائج.
- **V13-03 — One-command local run/reset docs:** كتابة تشغيل وإعادة ضبط محليين والتحقق منهما من بيئة نظيفة.
- **V13-04 — README/diagrams/demo script:** شرح المنتج والمعمارية وخطوات demo والحدود بوضوح.
- **V13-05 — Reproducible chart generation:** إعادة توليد الرسوم من raw experiment results.
- **V13-06 — Release/change log/hosting decision:** تجهيز release وسجل التغييرات واتخاذ قرار الاستضافة من غير تكلفة غير معتمدة.

**بوابة V13:** تشغيل محلي موثق، فحوص CI مكتملة، نتائج قابلة لإعادة الإنتاج، وحدود الادعاءات واضحة.

## قواعد قراءة الـRoadmap

- `D0-02A` مهمة إضافية ضمن V0، ولهذا عدد V0 الفعلي 14 صفًا رغم نطاق IDs الأساسي D0-01 إلى D0-13.
- V10-01 إلى V10-03 نُقلت مهام SignalR الأساسية إلى V3؛ لذلك V10 يحتوي V10-04 وV10-05 فقط.
- الـcut line يسمح بتأجيل بعض التجارب الاختيارية عند إعادة التخطيط، لكن لا يسمح بتجاوز بوابات السلامة أو الادعاء بأن تجربة مؤجلة قد اكتملت.
- الأرقام هنا تعدّ مهام الـbacklog، ولا تعني أن النظام جاهز للإنتاج أو لتحمل سعة معينة.
